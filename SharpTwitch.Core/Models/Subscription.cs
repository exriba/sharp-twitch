using SharpTwitch.Core.Enums;
using System.Text.Json.Serialization;

namespace SharpTwitch.Core.Models
{
    public class Subscription
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int Cost { get; set; }
        public Condition Condition { get; set; } = new Condition();
        public Transport Transport { get; set; } = new Transport();
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public SubscriptionType SubscriptionType
        {
            get
            {
                var subscriptionType = Type.Replace(".", "_");
                return Enum.Parse<SubscriptionType>(subscriptionType, true);
            }
        }

        [JsonIgnore]
        public SubscriptionStatus SubscriptionStatus => Enum.Parse<SubscriptionStatus>(Status, true);
    }
}
