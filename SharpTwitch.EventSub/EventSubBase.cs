using SharpTwitch.Core.Enums;

namespace SharpTwitch.EventSub
{
    public abstract class EventSubBase
    {
        internal abstract void RaiseEvent(SubscriptionType subscriptionType, System.EventArgs args);
        internal abstract void RaiseErrorEvent(SubscriptionType subscriptionType, Exception exception);
    }
}
