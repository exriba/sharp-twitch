namespace SharpTwitch.EventSub.Core.EventArgs
{
    public abstract class EventSubEventArgs<T> : System.EventArgs where T : class
    {
        public T Notification { get; set; }

        protected EventSubEventArgs(T notification)
        {
            Notification = notification;
        }
    }
}
