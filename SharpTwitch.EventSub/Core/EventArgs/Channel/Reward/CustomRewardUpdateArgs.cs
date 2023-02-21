using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.EventSub.Core.EventArgs.Channel.Reward
{
    public class CustomRewardUpdateArgs : EventSubEventArgs<EventSubMessage<EventPayload<CustomReward>>>
    {
        public CustomRewardUpdateArgs(EventSubMessage<EventPayload<CustomReward>> notification) : base(notification)
        {
        }
    }
}
