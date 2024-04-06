using System.Text.Json;
using SharpTwitch.EventSub.Client;
using Microsoft.Extensions.Logging;
using SharpTwitch.EventSub.Core.Enums;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.EventArgs;
using SharpTwitch.EventSub.Core.EventMessageArgs;
using SharpTwitch.EventSub.Core.Handler;
using Ardalis.GuardClauses;
using SharpTwitch.Core.NamingPolicies;
using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.User;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Redemption;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Reward;

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
        public WebSocketClient webSocketClient { get; private set; } = new();
        public string SessionId { get; private set; } = string.Empty;
        private CancellationTokenSource _cancellationTokenSource = new();
        private ConnectionStatus _connectionStatus = ConnectionStatus.DISCONNECTED;
        #endregion

        #region Immutable fields
        private readonly ILogger<EventSub>? _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IDictionary<SubscriptionType, INotificationHandler> _notificationHandlerMap;
        #endregion

        public EventSub(IEnumerable<INotificationHandler> notificationHandlers, ILogger<EventSub>? logger = null)
        {
            Guard.Against.Null(notificationHandlers, nameof(notificationHandlers));

            _logger = logger;
            webSocketClient.OnDataMessage += OnDataMessage;
            webSocketClient.OnErrorMessage += OnErrorOcurred;
            _notificationHandlerMap = new Dictionary<SubscriptionType, INotificationHandler>();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };

            ConfigureHandlers(notificationHandlers);
        }

        /// <summary>
        /// Resets Mutable fields.
        /// </summary>
        private void Reset()
        {
            SessionId = string.Empty;
            _keepAliveTimeout = TimeSpan.Zero;
            _lastReceived = DateTimeOffset.MinValue;
            _connectionStatus = ConnectionStatus.DISCONNECTED;
            webSocketClient = new WebSocketClient();
            webSocketClient.OnDataMessage += OnDataMessage;
            webSocketClient.OnErrorMessage += OnErrorOcurred;
            _cancellationTokenSource = new CancellationTokenSource();
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
        /// <returns><see langword="true"/> if the websocket client connects successfully. Otherwise returns <see langword="false"/>.</returns>
        public async Task<bool> ConnectAsync(Uri? uri = null)
        {
            uri ??= new Uri(TWITCH_EVENTSUB_URL);

            await webSocketClient.ConnectAsync(uri).ConfigureAwait(false);

            if (!webSocketClient.Connected)
                return false;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(ConnectionCheckAsync, _cancellationTokenSource.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            return true;
        }

        /// <summary>
        /// Disconnects the websocket client from Twitch.
        /// </summary>
        /// <returns><see langword="true"/> if the websocket client disconnects successfully. Otherwise returns <see langword="false"/>.</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (!webSocketClient.Connected)
                return true;

            _cancellationTokenSource.Cancel();
            return await webSocketClient.DisconnectAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Reconnects the websocket client to Twitch.
        /// </summary>
        /// <param name="uri">uri (Optional)</param>
        /// <returns><see langword="true"/> if the websocket client connects successfully. Otherwise returns <see langword="false"/>.</returns>
        public async Task<bool> ReconnectAsync(Uri? uri = null)
        {
            uri ??= new Uri(TWITCH_EVENTSUB_URL);

            if (_connectionStatus is ConnectionStatus.RECONNECTION_REQUESTED)
            {
                var webSocketClient = new WebSocketClient();
                webSocketClient.OnDataMessage += OnDataMessage;
                webSocketClient.OnErrorMessage += OnErrorMessage;

                await webSocketClient.ConnectAsync(uri).ConfigureAwait(false);

                if (!webSocketClient.Connected)
                    return false;

                for (var i = 0; i < 200; i++)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    if (_connectionStatus is ConnectionStatus.CONNECTED)
                    {
                        var oldWebSocketClient = this.webSocketClient;
                        this.webSocketClient = webSocketClient;
                        await oldWebSocketClient.DisposeAsync();

                        return true;
                    }

                    await Task.Delay(100);
                }

                _logger?.LogError("Unable to reconnect websocket connection for session {SessionId}.", SessionId);

                return false;
            }

            await DisposeAsync().ConfigureAwait(false);
            return await ConnectAsync(uri);
        }

        private async Task ConnectionCheckAsync()
        {
            while (webSocketClient.Connected && !_cancellationTokenSource.IsCancellationRequested)
            {
                var lastReceived = _lastReceived.Add(_keepAliveTimeout);

                if (lastReceived != DateTimeOffset.MinValue && lastReceived < DateTimeOffset.Now)
                    break;

                await Task.Delay(TimeSpan.FromSeconds(1), _cancellationTokenSource.Token);
            }

            await DisconnectAsync();

            var closeDetails = webSocketClient.GetCloseDetails();
            var disconnectedMessage = new ClientDisconnectedArgs()
            {
                WebSocketCloseStatus = closeDetails.CloseStatus,
                CloseStatusDescription = closeDetails.Description ?? string.Empty,
            };
            OnClientDisconnected?.Invoke(this, disconnectedMessage);
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

            SessionId = data.Payload.Session.Id;
            var keepAliveTimeout = data.Payload.Session.KeepaliveTimeoutSeconds + data.Payload.Session.KeepaliveTimeoutSeconds * 0.2;
            _keepAliveTimeout = keepAliveTimeout > 0 ? TimeSpan.FromSeconds(keepAliveTimeout) : TimeSpan.FromSeconds(10);

            _logger?.LogDebug("New session {session} started.", SessionId);
            OnClientConnected?.Invoke(this, new ClientConnectedArgs { ReconnectionRequested = _connectionStatus == ConnectionStatus.RECONNECTION_REQUESTED });
            _connectionStatus = ConnectionStatus.CONNECTED;
        }

        /// <summary>
        /// Handles TwitchEventSub reconnect message.
        /// </summary>
        /// <param name="jsonDocument"></param>
        private void HandleReconnect(JsonDocument jsonDocument)
        {
            var data = jsonDocument.Deserialize<EventSubMessage<SessionPayload>>(_jsonSerializerOptions)!;

            _connectionStatus = ConnectionStatus.RECONNECTION_REQUESTED;
            _logger?.LogWarning("Reconnection requested for session {sessionId}.", SessionId);
            Task.Run(async () => await ReconnectAsync(new Uri(data.Payload.Session.ReconnectUrl!)));
        }

        /// <summary>
        /// Handles Twitch EventSub keep alive message.
        /// </summary>
        /// <param name="jsonDocument">message</param>
        private void HandleKeepAlive(JsonDocument jsonDocument)
        {
            _logger?.LogDebug("{message}", jsonDocument);
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

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await webSocketClient.DisposeAsync();
            Reset();
        }
    }
}