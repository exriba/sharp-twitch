using SharpTwitch.Core.Models;

namespace SharpTwitch.EventSub.Core.Models
{
    public class SubscriptionPayload : IPayload
    {
        public Subscription Subscription { get; set; } = new Subscription();
    }
}
