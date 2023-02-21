namespace SharpTwitch.EventSub.Core.Models.Stream
{
    /// <summary>
    /// https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamonline
    /// </summary>
    public class StreamOnline : StreamBase
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
    }
}
