using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.SubscriptionTypes.Channel.Redemption;

namespace SharpTwitch.EventSub.Core.EventArgs.Channel.Redemption
{
    public class CustomRewardRedemptionArgs : EventSubEventArgs<EventSubMessage<EventPayload<ChannelPointsCustomRewardRedemption>>>
    {
        public CustomRewardRedemptionArgs(EventSubMessage<EventPayload<ChannelPointsCustomRewardRedemption>> notification) : base(notification)
        {
        }
    }
}
