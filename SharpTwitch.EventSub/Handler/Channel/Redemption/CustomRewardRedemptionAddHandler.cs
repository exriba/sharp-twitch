using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Redemption;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.SubscriptionTypes.Channel.Redemption;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Channel.Redemption
{
    public class CustomRewardRedemptionAddHandler : INotificationHandler
    {
        public SubscriptionType SubscriptionType => SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD;

        public void Handle(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<ChannelPointsCustomRewardRedemption>>>(jsonSerializerOptions);

            if (notification is null)
                throw new ArgumentException("Invalid Json string.");

            try
            {
                eventSubBase.RaiseEvent(SubscriptionType, new CustomRewardRedemptionArgs(notification));
            }
            catch (Exception ex)
            {
                ex.Data.Add("JSON", jsonDocument);
                eventSubBase.RaiseErrorEvent(SubscriptionType, ex);
            }
        }
    }
}
