using SharpTwitch.Core.Enums;
using System.Text.Json;

namespace SharpTwitch.EventSub.Core.Handler
{
    /// <summary>
    /// Defines the EventSub notification message handler.
    /// </summary>
    public interface INotificationHandler
    {
        /// <summary>
        /// The Subscription Type
        /// </summary>
        SubscriptionType SubscriptionType { get; }

        /// <summary>
        /// Creates and raises an EventSub message to be handled by the corresponding subscription type handler. 
        /// </summary>
        /// <param name="eventSubBase">event sub</param>
        /// <param name="jsonDocument">json document</param>
        /// <param name="jsonSerializerOptions">json serializer options</param>
        void Raise(EventSubBase eventSubBase, JsonDocument jsonDocument, JsonSerializerOptions? jsonSerializerOptions);
    }
}
