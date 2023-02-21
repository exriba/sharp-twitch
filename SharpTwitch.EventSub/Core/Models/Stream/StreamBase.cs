namespace SharpTwitch.EventSub.Core.Models.Stream
{
    public abstract class StreamBase
    {
        public string BroadcasterUserId { get; set; } = string.Empty;
        public string BroadcasterUserLogin { get; set; } = string.Empty;
        public string BroadcasterUserName { get; set; } = string.Empty;
    }
}
