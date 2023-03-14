namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Twitch EventSub Subscription Status.
    /// </summary>
    public enum SubscriptionStatus
    {
        /// <summary>
        ///  Twitch has verified your callback and is able to send you notifications..
        /// </summary>
        ENABLED,
        /// <summary>
        /// The user mentioned in the subscription no longer exists.
        /// </summary>
        USER_REMOVED,
        /// <summary>
        /// Twitch revoked your subscription because the subscribed to subscription type and version is no longer supported.
        /// </summary>
        VERSION_REMOVED,
        /// <summary>
        /// The moderator that authorized the subscription is no longer one of the broadcaster’s moderators.
        /// </summary>
        MODERATOR_REMOVED,
        /// <summary>
        /// Twitch revoked your subscription because the users in the condition object revoked their authorization letting you get events on their behalf, or changed their password.
        /// </summary>
        AUTHORIZATION_REVOKED,
        /// <summary>
        /// The client closed the connection.
        /// </summary>
        WEBSOCKET_DISCONNECTED,
        /// <summary>
        /// The client failed to respond to a ping message.
        /// </summary>
        WEBSOCKET_FAILED_PING_PONG,
        /// <summary>
        /// The client sent a non-pong message. Clients may only send pong messages (and only in response to a ping message).
        /// </summary>
        WEBSOCKET_RECEIVED_INBOUND_TRAFFIC,
        /// <summary>
        /// The client failed to subscribe to events within the required time.
        /// </summary>
        WEBSOCKET_CONNECTION_UNUSED,
        /// <summary>
        /// The Twitch WebSocket server experienced an unexpected error.
        /// </summary>
        WEBSOCKET_INTERNAL_ERROR,
        /// <summary>
        /// The Twitch WebSocket server timed out writing the message to the client.
        /// </summary>
        WEBSOCKET_NETWORK_TIMEOUT,
        /// <summary>
        /// The Twitch WebSocket server experienced a network error writing the message to the client.
        /// </summary>
        WEBSOCKET_NETWORK_ERROR
    }

    public static class SubscriptionStatusExtensions
    {
        /// <summary>
        /// Converts a subscription status into a string.
        /// </summary>
        /// <param name="subscriptionStatus">subscription status</param>
        /// <returns>The string value of the subscription status</returns>
        /// <exception cref="ArgumentException">If subscription status is invalid</exception>
        public static string ConvertToString(this SubscriptionStatus subscriptionStatus)
        {
            return subscriptionStatus switch
            {
                SubscriptionStatus.ENABLED => "enabled",
                SubscriptionStatus.USER_REMOVED => "user_removed",
                SubscriptionStatus.VERSION_REMOVED => "version_removed",
                SubscriptionStatus.MODERATOR_REMOVED => "moderator_removed",
                SubscriptionStatus.AUTHORIZATION_REVOKED => "authorization_revoked",
                SubscriptionStatus.WEBSOCKET_DISCONNECTED => "websocket_disconnected",
                SubscriptionStatus.WEBSOCKET_FAILED_PING_PONG => "websocket_failed_ping_pong",
                SubscriptionStatus.WEBSOCKET_RECEIVED_INBOUND_TRAFFIC => "websocket_received_inbound_traffic",
                SubscriptionStatus.WEBSOCKET_CONNECTION_UNUSED => "websocket_connection_unused",
                SubscriptionStatus.WEBSOCKET_INTERNAL_ERROR => "websocket_internal_error",
                SubscriptionStatus.WEBSOCKET_NETWORK_TIMEOUT => "websocket_network_timeout",
                SubscriptionStatus.WEBSOCKET_NETWORK_ERROR => "websocket_network_error",
                _ => throw new ArgumentException("Invalid Subscription Status", nameof(subscriptionStatus)),
            };
        }
    }
}
