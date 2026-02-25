using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.Models.Stream;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Stream
{
    /// <summary>
    /// Stream Offline Notification Handler.
    /// </summary>
    internal class StreamOfflineHandler : INotificationHandler
    {
        /// <inheritdoc/>
        public SubscriptionType SubscriptionType => SubscriptionType.STREAM_OFFLINE;

        /// <inheritdoc/>
        public void Raise(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            try
            {
                var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<StreamOffline>>>(jsonSerializerOptions);

                if (notification is not null)
                    eventSubBase.RaiseEvent(SubscriptionType, new StreamOfflineArgs(notification));
            }
            catch (Exception ex)
            {
                ex.Data.Add("JSON", jsonDocument);
                eventSubBase.RaiseErrorEvent(SubscriptionType, ex);
            }
        }
    }
}
