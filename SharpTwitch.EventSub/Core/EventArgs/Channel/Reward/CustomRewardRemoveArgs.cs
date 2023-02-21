using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.EventSub.Core.EventArgs.Channel.Reward
{
    public class CustomRewardRemoveArgs : EventSubEventArgs<EventSubMessage<EventPayload<CustomReward>>>
    {
        public CustomRewardRemoveArgs(EventSubMessage<EventPayload<CustomReward>> notification) : base(notification)
        {
        }
    }
}
