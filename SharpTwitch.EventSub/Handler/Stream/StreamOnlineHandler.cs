using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models.Stream;
using SharpTwitch.EventSub.Core.Models;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Stream
{
    /// <summary>
    /// Stream Online Notification Handler.
    /// </summary>
    internal class StreamOnlineHandler : INotificationHandler
    {
        /// <inheritdoc/>
        public SubscriptionType SubscriptionType => SubscriptionType.STREAM_ONLINE;

        /// <inheritdoc/>
        public void Raise(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            try
            {
                var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<StreamOnline>>>(jsonSerializerOptions);
                
                if (notification is not null)
                    eventSubBase.RaiseEvent(SubscriptionType, new StreamOnlineArgs(notification));
            }
            catch (Exception ex)
            {
                ex.Data.Add("JSON", jsonDocument);
                eventSubBase.RaiseErrorEvent(SubscriptionType, ex);
            }
        }
    }
}
