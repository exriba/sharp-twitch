using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.Models.Stream;

namespace SharpTwitch.EventSub.Core.EventArgs.Stream
{
    public class StreamOnlineArgs : EventSubEventArgs<EventSubMessage<EventPayload<StreamOnline>>>
    {
        public StreamOnlineArgs(EventSubMessage<EventPayload<StreamOnline>> notification) : base(notification)
        {
        }
    }
}
