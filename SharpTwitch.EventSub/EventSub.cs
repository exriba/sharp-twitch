using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
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
        private const string TWITCH_EVENTSUB_URL = "wss://eventsub.wss.twitch.tv/ws";
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

        #region Mutable fields
        private TimeSpan _keepAliveTimeout;
        private DateTimeOffset _lastReceived;
        private CancellationTokenSource _cancellationTokenSource = new();
        private TaskCompletionSource<bool> _connectionCompletionSource = new();

        public WebSocketClient WebSocketClient { get; private set; }
        public string SessionId { get; private set; } = string.Empty;
        #endregion

        #region Immutable fields
        private readonly ILogger<EventSub>? _logger;
        private readonly IDictionary<SubscriptionType, INotificationHandler> _notificationHandlerMap = new Dictionary<SubscriptionType, INotificationHandler>();
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
        #endregion

        public EventSub(IEnumerable<INotificationHandler> notificationHandlers, ILogger<EventSub>? logger = null)
        {
            Guard.Against.Null(notificationHandlers, nameof(notificationHandlers));
            WebSocketClient = new WebSocketClient();
            ConfigureHandlers(notificationHandlers);
            _logger = logger;
        }

        /// <summary>
        /// Loads registered notification handlers.
        /// </summary>
        /// <param name="notificationHandlers">notification handlers</param>
        private void ConfigureHandlers(IEnumerable<INotificationHandler> notificationHandlers)
        {
            foreach (var handler in notificationHandlers)
                _notificationHandlerMap.TryAdd(handler.SubscriptionType, handler);
        }

        /// <summary>
        /// Connects the websocket client to Twitch.
        /// </summary>
        /// <param name="uri">uri (Optional)</param>
        /// <param name="cancellationToken">the cancellation token</param>
        public async Task ConnectAsync(Uri? uri = null, CancellationToken cancellationToken = default)
        {
            uri ??= new Uri(TWITCH_EVENTSUB_URL);
            WebSocketClient = new WebSocketClient();
            WebSocketClient.OnDataMessage += OnDataMessage;
            WebSocketClient.OnErrorMessage += OnErrorOcurred;
            _cancellationTokenSource = _cancellationTokenSource.IsCancellationRequested ? new() : _cancellationTokenSource;
            var token = _cancellationTokenSource.Token;

            await WebSocketClient.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);

            _ = Task.Run(() => ConnectionCheckAsync(WebSocketClient, token), token);
        }

        /// <summary>
        /// Disconnects the websocket client from Twitch.
        /// </summary>
        /// <param name="cancellationToken">the cancellation token</param>
        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            if (!WebSocketClient.Connected)
                return;

            _cancellationTokenSource.Cancel();
            await WebSocketClient.DisconnectAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task ConnectionCheckAsync(WebSocketClient webSocketClient, CancellationToken cancellationToken = default)
        {
            try
            {
                while (webSocketClient.Connected)
                {
                    var lastReceived = _lastReceived.Add(_keepAliveTimeout);

                    if (lastReceived != DateTimeOffset.MinValue && lastReceived < DateTimeOffset.Now)
                        break;

                    var delay = _keepAliveTimeout == TimeSpan.Zero ? TimeSpan.FromSeconds(10) : _keepAliveTimeout;
                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogDebug("ConnectionCheck has been canceled due to disconnection or reconnection.");
                _cancellationTokenSource.Dispose();
            }

            if (!webSocketClient.Connected)
            {
                var closeDetails = webSocketClient.GetCloseDetails();
                var disconnectedMessage = new ClientDisconnectedArgs()
                {
                    WebSocketCloseStatus = closeDetails.CloseStatus,
                    CloseStatusDescription = closeDetails.Description ?? string.Empty,
                };
                OnClientDisconnected?.Invoke(this, disconnectedMessage);
            }
        }

        /// <summary>
        /// Reconnects the websocket client to Twitch.
        /// </summary>
        /// <param name="uri">uri (Optional)</param>
        /// <param name="cancellationToken">the cancellation token</param>
        public async Task ReconnectAsync(Uri? uri = null, CancellationToken cancellationToken = default)
        {
            uri ??= new Uri(TWITCH_EVENTSUB_URL);
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
            await ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
        }

        private async Task ReconnectAsync(Uri? uri = null)
        {
            uri ??= new Uri(TWITCH_EVENTSUB_URL);
            _cancellationTokenSource.Cancel();

            var webSocketClient = new WebSocketClient();
            webSocketClient.OnDataMessage += OnDataMessage;
            webSocketClient.OnErrorMessage += OnErrorMessage;

            await webSocketClient.ConnectAsync(uri);

            _cancellationTokenSource = new CancellationTokenSource();
            _connectionCompletionSource = new TaskCompletionSource<bool>();
            var token = _cancellationTokenSource.Token;

            _ = Task.Run(() => ConnectionCheckAsync(webSocketClient, token), token);

            var connectionTask = _connectionCompletionSource.Task;
            var timeoutTask = Task.Delay(_keepAliveTimeout);

            var completedTask = await Task.WhenAny(connectionTask, timeoutTask);

            if (completedTask == connectionTask && connectionTask.Result)
            {
                var oldWebSocketClient = WebSocketClient;
                WebSocketClient = webSocketClient;
                await oldWebSocketClient.DisconnectAsync();
                return;
            }

            _connectionCompletionSource.SetResult(false);
            _logger?.LogError("Connection Timeout. Unable to reconnect websocket for session {SessionId}.", SessionId);
        }

        /// <summary>
        /// Invokes Error event handler.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">error message arguments</param>
        private void OnErrorOcurred(object? sender, ErrorMessageArgs e)
        {
            OnErrorMessage?.Invoke(sender, e);
        }

        /// <summary>
        /// Invokes the appropiate handler depending on the message type.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event message args</param>
        private void OnDataMessage(object? sender, T e)
        {
            _lastReceived = DateTimeOffset.Now;

            if (e.Message is null)
                return;

            var jsonDocument = JsonDocument.Parse(e.Message);
            var messageType = jsonDocument.RootElement.GetProperty(METADATA).GetProperty(MESSAGE_TYPE).GetString();

            if (messageType is not null)
            {
                var type = Enum.Parse<MessageType>(messageType, true);

                switch (type)
                {
                    case MessageType.SESSION_WELCOME:
                        HandleWelcome(jsonDocument);
                        break;
                    case MessageType.SESSION_RECONNECT:
                        HandleReconnect(jsonDocument);
                        break;
                    case MessageType.SESSION_KEEPALIVE:
                        HandleKeepAlive(jsonDocument);
                        break;
                    case MessageType.NOTIFICATION:
                        HandleNotification(jsonDocument);
                        break;
                    case MessageType.REVOCATION:
                        HandleRevocation(jsonDocument);
                        break;
                    default:
                        _logger?.LogWarning("Unknown message type: {messageType}.", messageType);
                        break;
                }
            }
        }

        /// <summary>
        /// Handles Twitch EventSub welcome message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleWelcome(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SessionPayload>>(_jsonSerializerOptions)!;

            var keepAliveTimeout = data.Payload.Session.KeepaliveTimeoutSeconds * 1.2;
            _keepAliveTimeout = TimeSpan.FromSeconds(keepAliveTimeout!.Value);
            SessionId = data.Payload.Session.Id;

            _logger?.LogDebug("New session {sessionId} started.", SessionId);

            var reconnectionRequested = data.Metadata.MetadataMessageType == MessageType.SESSION_RECONNECT;
            OnClientConnected?.Invoke(this, new ClientConnectedArgs { ReconnectionRequested = reconnectionRequested });
            _connectionCompletionSource.SetResult(true);
        }

        /// <summary>
        /// Handles TwitchEventSub reconnect message.
        /// </summary>
        /// <param name="jsonDocument"></param>
        private void HandleReconnect(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SessionPayload>>(_jsonSerializerOptions)!;

            _logger?.LogWarning("Reconnection requested for session {sessionId}.", SessionId);

            var reconnectionUri = new Uri(data.Payload.Session.ReconnectUrl!);
            Task.Run(() => ReconnectAsync(reconnectionUri));
        }

        /// <summary>
        /// Handles Twitch EventSub keep alive message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleKeepAlive(JsonDocument jsonDocument)
        {
            var timeStamp = DateTimeOffset.UtcNow;
            _logger?.LogDebug("Heartbeat {timestamp}", timeStamp);
        }

        /// <summary>
        /// Handles Twitch EventSub notification message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleNotification(JsonDocument jsonDocument)
        {
            var metadataDocument = jsonDocument.RootElement.GetProperty(METADATA);
            var metadata = metadataDocument.Deserialize<Metadata>(_jsonSerializerOptions);

            if (metadata is not null && _notificationHandlerMap.TryGetValue(metadata.MetadataSubscriptionType, out var handler))
                handler.Raise(this, jsonDocument, _jsonSerializerOptions);
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
        internal override void RaiseEvent(SubscriptionType subscriptionType, System.EventArgs args)
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

            OnErrorMessage?.Invoke(this, errorMessage);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await WebSocketClient.DisposeAsync().ConfigureAwait(false);
        }
    }
}