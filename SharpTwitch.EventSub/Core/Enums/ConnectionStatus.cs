namespace SharpTwitch.EventSub.Core.Enums
{
    public enum ConnectionStatus
    {
        CONNECTED,
        DISCONNECTED,
        RECONNECTION_REQUESTED
    }

    public static class ConnectionStatusExtensions
    {
        public static string ConvertToString(this ConnectionStatus connectionStatus)
        {
            return connectionStatus switch
            {
                ConnectionStatus.CONNECTED => "connected",
                ConnectionStatus.DISCONNECTED => "disconnected",
                ConnectionStatus.RECONNECTION_REQUESTED => "reconnection_requested",
                _ => throw new ArgumentException("Invalid Connection Status", nameof(connectionStatus)),
            };
        }
    }
}
