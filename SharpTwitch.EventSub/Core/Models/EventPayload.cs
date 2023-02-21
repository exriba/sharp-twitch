using SharpTwitch.Core.Models;

namespace SharpTwitch.EventSub.Core.Models
{
    public class EventPayload<T> : IPayload where T : class
    {
        public Subscription Subscription { get; set; } = new Subscription();
        public T Event { get; set; }
    }
}
