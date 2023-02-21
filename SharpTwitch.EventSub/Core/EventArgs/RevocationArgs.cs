using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.Enums;

namespace SharpTwitch.EventSub.Core.EventArgs
{
    public class RevocationArgs : System.EventArgs
    {
        public string BroadcasterUserId { get; internal set; } = string.Empty;
        public MessageType MessageType { get; internal set; }
        public SubscriptionType SubscriptionType { get; internal set; }
        public SubscriptionStatus SubscriptionStatus { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
