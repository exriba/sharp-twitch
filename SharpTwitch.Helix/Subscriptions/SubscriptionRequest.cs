using SharpTwitch.Core.Models;

namespace SharpTwitch.Helix.Subscriptions
{
    public class SubscriptionRequest
    {
        public string Type { get; set; } = string.Empty;
        public string Version { get; set; } = "1";
        public Condition Condition { get; set; } = new Condition();
        public Transport Transport { get; set; } = new Transport();
    }
}
