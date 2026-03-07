using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.NamingPolicies;
using SharpTwitch.EventSub.Client;
using SharpTwitch.EventSub.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Redemption;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Reward;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.EventArgs.User;
using SharpTwitch.EventSub.Core.EventMessageArgs;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using System.Text.Json;

namespace SharpTwitch.EventSub
{
    /// <summary>
    /// Twitch EventSub.
    /// </summary>
    public class EventSub : EventSubBase, IAsyncDisposable
    {
        #region Constants
        private const string METADATA = "metadata";
        private const string MESSAGE_TYPE = "message_type";
        #endregion

        #region EventHandlers
        public event EventHandler<StreamOnlineArgs>? OnStreamOnline;
        public event EventHandler<StreamOfflineArgs>? OnStreamOffline;
        public event EventHandler<UserUpdateArgs>? OnUserUpdate;
        public event EventHandler<RevocationArgs>? OnRevocation;
        public event EventHandler<ErrorMessageArgs>? OnErrorMessage;
        public event EventHandler<ClientConnectedArgs>? OnClientConnected;
        public event EventHandler<ClientDisconnectedArgs>? OnClientDisconnected;
        public event EventHandler<CustomRewardAddArgs>? OnCustomRewardAdd;
        public event EventHandler<CustomRewardUpdateArgs>? OnCustomRewardUpdate;
        public event EventHandler<CustomRewardRemoveArgs>? OnCustomRewardRemove;
        public event EventHandler<CustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemption;
        #endregion

        #region Immutable fields
        private readonly ILogger<EventSub> _logger;
        private readonly object _lastReceivedLock = new();
        private readonly SemaphoreSlim _reconnectLock = new(1, 1);
        private static readonly Uri DefaultUri = new("wss://eventsub.wss.twitch.tv/ws");
        private readonly Dictionary<MessageType, Action<JsonDocument>> _messageHandlers;
        private readonly IDictionary<SubscriptionType, INotificationHandler> _notificationHandlerMap;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
        #endregion

        #region Mutable fields
        private TimeSpan _keepAliveTimeout;
        private DateTimeOffset _lastReceived;
        private CancellationTokenSource _connectionCancellationSource = new();
        private TaskCompletionSource<bool> _connectionCompletionSource = new();
        private IWebSocketClient WebSocketClient;
        public string SessionId { get; private set; } = string.Empty;
        #endregion


        /// <summary>
        /// Gets a value indicating whether the underlying WebSocket client is currently connected.
        /// </summary>
        public bool Connected => WebSocketClient is not null && WebSocketClient.Connected;

        /// <summary>
        /// Gets a value indicating whether the underlying WebSocket client is in a faulted state.
        /// </summary>
        public bool Faulted => WebSocketClient is not null && WebSocketClient.Faulted;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSub"/> class.
        /// </summary>
        /// <param name="notificationHandlers">A collection of notification handlers that will process incoming messages.</param>
        /// <param name="webSocketClient">A Websocket Client capable of receiving and pushing notification messages.</param>
        /// <param name="logger">An optional logger instance used for diagnostic and error logging.</param>
        /// <remarks>
        /// This constructor configures the provided notification handlers,
        /// creates the underlying <see cref="WebSocketClient"/>, and subscribes
        /// to its data and error events.
        /// </remarks>
        public EventSub(IEnumerable<INotificationHandler> notificationHandlers, IWebSocketClient? webSocketClient = null, ILogger<EventSub>? logger = null)
        {
            _logger = logger ?? NullLogger<EventSub>.Instance;
            _notificationHandlerMap = new Dictionary<SubscriptionType, INotificationHandler>();
            _messageHandlers = new Dictionary<MessageType, Action<JsonDocument>>
            {
                { MessageType.SESSION_WELCOME, HandleWelcome },
                { MessageType.SESSION_RECONNECT, HandleReconnect },
                { MessageType.SESSION_KEEPALIVE, HandleKeepAlive },
                { MessageType.NOTIFICATION, HandleNotification },
                { MessageType.REVOCATION, HandleRevocation }
            };

            ConfigureHandlers(notificationHandlers);
            WebSocketClient = webSocketClient ?? new WebSocketClient();
            WebSocketClient.OnDataMessage += OnDataMessage;
            WebSocketClient.OnErrorMessage += OnErrorOccurred;
        }

        /// <summary>
        /// Loads registered notification handlers.
        /// </summary>
        /// <param name="notificationHandlers">notification handlers</param>
        private void ConfigureHandlers(IEnumerable<INotificationHandler> notificationHandlers)
        {
            Guard.Against.Null(notificationHandlers, nameof(notificationHandlers));

            foreach (var handler in notificationHandlers)
                _notificationHandlerMap.TryAdd(handler.SubscriptionType, handler);
        }

        /// <summary>
        /// Connects the websocket client to Twitch.
        /// </summary>
        /// <param name="uri">uri (Optional)</param>
        /// <param name="cancellationToken">cancellation token</param>
        public async Task ConnectAsync(Uri? uri = null, CancellationToken cancellationToken = default)
        {
            if (Connected)
            {
                _logger.LogDebug("Already connected to Twitch EventSub.");
                return;
            }

            if (Faulted)
            {
                _logger.LogWarning("Previous connection was faulted. Disconnecting...");
                await DisconnectAsync(cancellationToken).ConfigureAwait(false);
            }

            if (_connectionCancellationSource.IsCancellationRequested)
                _connectionCancellationSource = new CancellationTokenSource();

            uri ??= DefaultUri;
            await WebSocketClient.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
            _ = Task.Run(() => ConnectionCheckAsync(uri, _connectionCancellationSource.Token), _connectionCancellationSource.Token);
        }

        /// <summary>
        /// Disconnects the websocket client from Twitch.
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            if (!Connected)
            {
                _logger.LogDebug("Already disconnected from Twitch EventSub.");
                return;
            }

            _connectionCancellationSource.Cancel();
            await WebSocketClient.DisconnectAsync(cancellationToken).ConfigureAwait(false);
            _connectionCancellationSource.Dispose();
        }

        private async Task ConnectionCheckAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Wait in case _keepAliveTimeout is not set yet
                var delay = _keepAliveTimeout == TimeSpan.Zero ? TimeSpan.FromSeconds(10) : _keepAliveTimeout;
                await Task.Delay(delay, cancellationToken);

                // Skip because we never received a message
                if (_lastReceived == default)
                    continue;

                var elapsed = TimeSpan.Zero;

                lock (_lastReceivedLock)
                {
                    elapsed = DateTimeOffset.UtcNow - _lastReceived;
                }

                if (elapsed > _keepAliveTimeout)
                {
                    _logger.LogWarning("EventSub keepalive timeout detected for session {SessionId}. " +
                        "Last message received {Elapsed}s ago.", SessionId, elapsed.TotalSeconds);

                    await ReconnectAsync(uri, CancellationToken.None);
                    return;
                }
            }
        }

        /// <summary>
        /// Reconnects the websocket client to Twitch.
        /// </summary>
        /// <param name="uri">uri (Optional)</param>
        /// <param name="cancellationToken">cancellation token</param>
        public async Task ReconnectAsync(Uri? uri = null, CancellationToken cancellationToken = default)
        {
            await _reconnectLock.WaitAsync();

            uri ??= DefaultUri;

            try
            {
                _connectionCancellationSource.Cancel();
                _connectionCancellationSource.Dispose();

                // Store the old client
                var oldWebSocketClient = WebSocketClient;

                // Create new WebSocket client
                var webSocketClient = new WebSocketClient();
                webSocketClient.OnDataMessage += OnDataMessage;
                webSocketClient.OnErrorMessage += OnErrorOccurred;

                await webSocketClient.ConnectAsync(uri, cancellationToken);

                // Reset cancellation and completion sources
                _connectionCancellationSource = new CancellationTokenSource();
                _connectionCompletionSource = new TaskCompletionSource<bool>();

                _ = Task.Run(() => ConnectionCheckAsync(uri, _connectionCancellationSource.Token), _connectionCancellationSource.Token);

                // Wait for connection completion or timeout
                var completedTask = await Task.WhenAny(_connectionCompletionSource.Task, Task.Delay(_keepAliveTimeout, cancellationToken));

                if (completedTask == _connectionCompletionSource.Task && _connectionCompletionSource.Task.Result)
                {
                    WebSocketClient = webSocketClient;
                }
                else
                {
                    _connectionCompletionSource.TrySetResult(false);
                    _logger.LogError("Connection Timeout. Unable to reconnect websocket for session {SessionId}.", SessionId);
                }

                oldWebSocketClient.OnDataMessage -= OnDataMessage;
                oldWebSocketClient.OnErrorMessage -= OnErrorOccurred;
                await oldWebSocketClient.DisconnectAsync(cancellationToken);
            }
            finally
            {
                _reconnectLock.Release();
            }
        }

        /// <summary>
        /// Invokes Error event handler.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">error message arguments</param>
        private void OnErrorOccurred(object? sender, ErrorMessageArgs e)
        {
            OnErrorMessage?.Invoke(sender, e);
        }

        /// <summary>
        /// Invokes the appropiate handler depending on the message type.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event message args</param>
        private void OnDataMessage(object? sender, DataMessageArgs e)
        {
            if (e.Message is null)
                return;

            lock (_lastReceivedLock)
            {
                _lastReceived = DateTimeOffset.UtcNow;
            }

            using var message = JsonDocument.Parse(e.Message);
            var metadata = message.RootElement.GetProperty(METADATA);
            var messageType = metadata.GetProperty(MESSAGE_TYPE).GetString();

            if (!Enum.TryParse<MessageType>(messageType, true, out var type))
            {
                _logger.LogWarning("Unknown message type: {messageType}", messageType);
                return;
            }

            if (!_messageHandlers.TryGetValue(type, out var handler))
            {
                _logger.LogWarning("No handler defined for message type: {messageType}", type);
                return;
            }

            handler(message);
        }

        /// <summary>
        /// Handles Twitch EventSub welcome message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleWelcome(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SessionPayload>>(_jsonSerializerOptions);

            if (data?.Payload?.Session is null)
            {
                _logger.LogError("Invalid welcome message: missing session data.");
                OnErrorOccurred(this, new ErrorMessageArgs
                {
                    Message = "Invalid welcome message structure.",
                    Exception = new InvalidOperationException("Missing session data.")
                });
                return;
            }

            // Keepalive timeout with 20% buffer
            var keepAliveTimeout = data.Payload.Session.KeepaliveTimeoutSeconds * 1.2;
            _keepAliveTimeout = TimeSpan.FromSeconds(keepAliveTimeout);
            SessionId = data.Payload.Session.Id;

            _logger.LogDebug("New EventSub session {SessionId} started.", SessionId);

            var reconnectionRequested = data.Metadata.MetadataMessageType == MessageType.SESSION_RECONNECT;
            OnClientConnected?.Invoke(this, new ClientConnectedArgs { ReconnectionRequested = reconnectionRequested });
            _connectionCompletionSource.TrySetResult(true);
        }

        /// <summary>
        /// Handles TwitchEventSub reconnect message.
        /// </summary>
        /// <param name="jsonDocument"></param>
        private void HandleReconnect(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SessionPayload>>(_jsonSerializerOptions);

            if (data?.Payload?.Session?.ReconnectUrl is null)
            {
                _logger.LogError("Invalid reconnect message: missing reconnect url.");
                OnErrorOccurred(this, new ErrorMessageArgs
                {
                    Message = "Invalid reconnect message structure.",
                    Exception = new InvalidOperationException("Missing reconnect url.")
                });
                return;
            }

            _logger.LogWarning("Reconnection requested for session {SessionId}.", data.Payload.Session.Id);

            var reconnectionUri = new Uri(data.Payload.Session.ReconnectUrl);

            _ = Task.Run(async () =>
            {
                try
                {
                    await ReconnectAsync(reconnectionUri, CancellationToken.None);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Reconnection attempt failed for session {SessionId}.", data.Payload.Session.Id);
                }
            });
        }

        /// <summary>
        /// Handles Twitch EventSub keep alive message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleKeepAlive(JsonDocument jsonDocument)
        {
            lock (_lastReceivedLock)
            {
                _lastReceived = DateTimeOffset.UtcNow;
            }

            _logger.LogTrace("Received keepalive for session {SessionId} at {TimeStamp}.", SessionId, _lastReceived);
        }

        /// <summary>
        /// Handles Twitch EventSub notification message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleNotification(JsonDocument jsonDocument)
        {
            if (!jsonDocument.RootElement.TryGetProperty(METADATA, out var metadataElement))
            {
                _logger.LogWarning("Notification message missing metadata. Ignored.");
                return;
            }

            var metadata = metadataElement.Deserialize<Metadata>(_jsonSerializerOptions);

            if (metadata is null)
            {
                _logger.LogWarning("Failed to deserialize notification metadata. Ignored.");
                return;
            }

            if (!_notificationHandlerMap.TryGetValue(metadata.MetadataSubscriptionType, out var handler))
            {
                _logger.LogDebug("No registered handler for subscription type {SubscriptionType}.", metadata.MetadataSubscriptionType);
                return;
            }

            try
            {
                handler.Raise(this, jsonDocument, _jsonSerializerOptions);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Notification handler threw an exception for subscription type {SubscriptionType}.", metadata.MetadataSubscriptionType);
            }
        }

        /// <summary>
        /// Handles TwitchEventSub revocation message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleRevocation(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SubscriptionPayload>>(_jsonSerializerOptions)!;

            var revocationArgs = new RevocationArgs
            {
                MessageType = data.Metadata.MetadataMessageType,
                SubscriptionType = data.Payload.Subscription.SubscriptionType,
                SubscriptionStatus = data.Payload.Subscription.SubscriptionStatus,
                BroadcasterUserId = data.Payload.Subscription.Condition.BroadcasterUserId,
                CreatedAt = data.Payload.Subscription.CreatedAt
            };

            OnRevocation?.Invoke(this, revocationArgs);
        }

        /// <inheritdoc/>
        internal override void RaiseEvent(SubscriptionType subscriptionType, EventArgs args)
        {
            switch (subscriptionType)
            {
                case SubscriptionType.STREAM_ONLINE:
                    OnStreamOnline?.Invoke(this, (StreamOnlineArgs)args);
                    break;
                case SubscriptionType.STREAM_OFFLINE:
                    OnStreamOffline?.Invoke(this, (StreamOfflineArgs)args);
                    break;
                case SubscriptionType.USER_UPDATE:
                    OnUserUpdate?.Invoke(this, (UserUpdateArgs)args);
                    break;
                case SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_ADD:
                    OnCustomRewardAdd?.Invoke(this, (CustomRewardAddArgs)args);
                    break;
                case SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE:
                    OnCustomRewardUpdate?.Invoke(this, (CustomRewardUpdateArgs)args);
                    break;
                case SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REMOVE:
                    OnCustomRewardRemove?.Invoke(this, (CustomRewardRemoveArgs)args);
                    break;
                case SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD:
                    OnChannelPointsCustomRewardRedemption?.Invoke(this, (CustomRewardRedemptionArgs)args);
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        internal override void RaiseErrorEvent(SubscriptionType subscriptionType, Exception exception)
        {
            var errorMessage = new ErrorMessageArgs
            {
                Exception = exception,
                Message = $"Error encountered while trying to handle {subscriptionType} notification."
            };

            try
            {
                OnErrorMessage?.Invoke(this, errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The error message handler itself threw an exception.");
            }

            _logger.LogError(exception, "Error handling {SubscriptionType} notification for session {SessionId}.", subscriptionType, SessionId);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            try
            {
                _connectionCancellationSource.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // Already disposed — safe to ignore
            }

            await DisconnectAsync(CancellationToken.None);
            await WebSocketClient.DisposeAsync().ConfigureAwait(false);
            _connectionCancellationSource.Dispose();
        }
    }
}