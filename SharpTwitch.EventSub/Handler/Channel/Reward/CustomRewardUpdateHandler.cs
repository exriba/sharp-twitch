using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Reward;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.Helix.Models.Channel.Reward;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Channel.Reward
{
    public class CustomRewardUpdateHandler : INotificationHandler
    {
        public SubscriptionType SubscriptionType => SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE;

        public void Handle(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<CustomReward>>>(jsonSerializerOptions);

            if (notification is null)
                throw new ArgumentException("Invalid Json string.");

            try
            {
                eventSubBase.RaiseEvent(SubscriptionType, new CustomRewardUpdateArgs(notification));
            }
            catch (Exception ex)
            {
                ex.Data.Add("JSON", jsonDocument);
                eventSubBase.RaiseErrorEvent(SubscriptionType, ex);
            }
        }
    }
}
