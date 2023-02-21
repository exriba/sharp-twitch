namespace SharpTwitch.EventSub.Core.EventArgs
{
    public class ClientConnectedArgs : System.EventArgs
    {
        public bool ReconnectionRequested { get; internal set; }
    }
}
