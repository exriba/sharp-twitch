using SharpTwitch.EventSub.Core.Models;
using SharpTwitch.EventSub.Core.Models.User;

namespace SharpTwitch.EventSub.Core.EventArgs.User
{
    public class UserUpdateArgs : EventSubEventArgs<EventSubMessage<EventPayload<UserUpdate>>>
    {
        public UserUpdateArgs(EventSubMessage<EventPayload<UserUpdate>> notification) : base(notification)
        {
        }
    }
}
