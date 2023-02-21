using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.EventSub.Core.EventArgs.Channel.Reward
{
    public class CustomRewardAddArgs : EventSubEventArgs<EventSubMessage<EventPayload<CustomReward>>>
    {
        public CustomRewardAddArgs(EventSubMessage<EventPayload<CustomReward>> notification) : base(notification)
        {
        }
    }
}
