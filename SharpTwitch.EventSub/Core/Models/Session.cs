namespace SharpTwitch.EventSub.Core.Models
{
    public class Session
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int KeepaliveTimeoutSeconds { get; set; }
        public string? ReconnectUrl { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}
