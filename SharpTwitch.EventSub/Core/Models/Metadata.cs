using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.Enums;
using System.Text.Json.Serialization;

namespace SharpTwitch.EventSub.Core.Models
{
    public class Metadata
    {
        public string MessageId { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public DateTime MessageTimestamp { get; set; }
        public string? SubscriptionType { get; set; }
        public string? Version { get; set; }

        [JsonIgnore]
        public MessageType MetadataMessageType => Enum.Parse<MessageType>(MessageType, true);

        [JsonIgnore]
        public SubscriptionType MetadataSubscriptionType
        {
            get
            {
                var subscriptionType = SubscriptionType?.Replace(".", "_"); 
                return Enum.Parse<SubscriptionType>(subscriptionType, true);
            }
        }
    }
}
