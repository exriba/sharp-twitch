using SharpTwitch.Core.Models;

namespace SharpTwitch.Helix.Subscriptions
{
    /// <summary>
    /// Twitch <see href="https://dev.twitch.tv/docs/api/reference/#create-eventsub-subscription">Subscription Request</see>
    /// </summary>
    public class SubscriptionRequest
    {
        public string Type { get; set; } = string.Empty;
        public string Version { get; set; } = "1";
        public Condition Condition { get; set; } = new Condition();
        public Transport Transport { get; set; } = new Transport();
    }
}
