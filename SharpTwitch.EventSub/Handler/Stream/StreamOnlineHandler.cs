using SharpTwitch.Core.Enums;
using SharpTwitch.EventSub.Core.EventArgs.Stream;
using SharpTwitch.EventSub.Core.Handler;
using SharpTwitch.EventSub.Core.Models.Stream;
using SharpTwitch.EventSub.Core.Models;
using System.Text.Json;

namespace SharpTwitch.EventSub.Handler.Stream
{
    internal class StreamOnlineHandler : INotificationHandler
    {
        public SubscriptionType SubscriptionType => SubscriptionType.STREAM_ONLINE;

        public void Handle(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions)
        {
            var notification = jsonDocument.Deserialize<EventSubMessage<EventPayload<StreamOnline>>>(jsonSerializerOptions);

            if (notification is null)
                throw new ArgumentException("Invalid Json string.");

            try
            {
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
