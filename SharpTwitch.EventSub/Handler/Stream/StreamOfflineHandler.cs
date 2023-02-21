using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models;
using System.Text.Json;
using SharpTwitch.EventSub.Core.Models.Stream;
using SharpTwitch.EventSub.Core.EventArgs.Stream;

namespace SharpTwitch.EventSub.Handler.Stream
{
    internal class StreamOfflineHandler : INotificationHandler
    {
        public SubscriptionType SubscriptionType => SubscriptionType.STREAM_OFFLINE;

        public void Handle(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<StreamOffline>>>(jsonSerializerOptions);

            if (notification is null)
                throw new ArgumentException("Invalid Json string.");

            try
            {
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
