namespace SharpTwitch.EventSub.Core.Enums
{
    public enum MessageType
    {
        SESSION_WELCOME,
        SESSION_RECONNECT,
        SESSION_KEEPALIVE,
        NOTIFICATION,
        REVOCATION
    }

    public static class MessageTypeExtensions
    {
        public static string ConvertToString(this MessageType messageType)
        {
            return messageType switch
            {
                MessageType.SESSION_WELCOME => "session_welcome",
                MessageType.SESSION_RECONNECT => "session_reconnect",
                MessageType.SESSION_KEEPALIVE => "session_keepalive",
                MessageType.NOTIFICATION => "notification",
                MessageType.REVOCATION => "revocation",
                _ => throw new ArgumentException("Invalid Message Type", nameof(messageType)),
            };
        }
    }
}
