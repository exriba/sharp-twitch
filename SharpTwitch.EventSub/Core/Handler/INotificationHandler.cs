using System.Text.Json;
using SharpTwitch.Core.Enums;

namespace SharpTwitch.EventSub.Core.Handler
{
    public interface INotificationHandler
    {
        SubscriptionType SubscriptionType { get; }

        void Handle(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions);
    }
}
