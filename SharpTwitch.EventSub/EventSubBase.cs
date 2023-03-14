using SharpTwitch.Core.Enums;

namespace SharpTwitch.EventSub
{
    /// <summary>
    /// Base EventSub implementation.
    /// </summary>
    public abstract class EventSubBase
    {
        /// <summary>
        /// Invokes event handler for the given subscription type.
        /// </summary>
        /// <param name="subscriptionType">subscription type</param>
        /// <param name="args">event arguments</param>
        internal abstract void RaiseEvent(SubscriptionType subscriptionType, EventArgs args);

        /// <summary>
        /// Invokes exception event handler for the given subscription type. 
        /// </summary>
        /// <param name="subscriptionType">subscription type</param>
        /// <param name="exception">exception</param>
        internal abstract void RaiseErrorEvent(SubscriptionType subscriptionType, Exception exception);
    }
}
