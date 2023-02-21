using SharpTwitch.EventSub.Core.Models.Stream;
using SharpTwitch.EventSub.Core.Models;

namespace SharpTwitch.EventSub.Core.EventArgs.Stream
{
    public class StreamOfflineArgs : EventSubEventArgs<EventSubMessage<EventPayload<StreamOffline>>>
    {
        public StreamOfflineArgs(EventSubMessage<EventPayload<StreamOffline>> notification) : base(notification)
        {
        }
    }
}
