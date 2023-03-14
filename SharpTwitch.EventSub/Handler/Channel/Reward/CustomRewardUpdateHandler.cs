using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Channel.Reward;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.Helix.Models.Channel.Reward;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Channel.Reward
{
    /// <summary>
    /// Custom Reward Updated Notification Handler.
    /// </summary>
    internal class CustomRewardUpdateHandler : INotificationHandler
    {
        /// <inheritdoc/>
        public SubscriptionType SubscriptionType => SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE;

        /// <inheritdoc/>
        public void Raise(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            try
            {
                var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<CustomReward>>>(jsonSerializerOptions);

                if (notification is not null)
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
