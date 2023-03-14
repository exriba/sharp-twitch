using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.User;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.Models.User;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.User
{
    /// <summary>
    /// User Update Notification Handler.
    /// </summary>
    internal class UserUpdateHandler : INotificationHandler
    {
        /// <inheritdoc/>
        public SubscriptionType SubscriptionType => SubscriptionType.USER_UPDATE;

        /// <inheritdoc/>
        public void Raise(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            try
            {
                var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<UserUpdate>>>(jsonSerializerOptions);

                if (notification is not null)
                    eventSubBase.RaiseEvent(SubscriptionType, new UserUpdateArgs(notification));
            }
            catch (Exception ex)
            {
                ex.Data.Add("JSON", jsonDocument);
                eventSubBase.RaiseErrorEvent(SubscriptionType, ex);
            }
        }
    }
}
