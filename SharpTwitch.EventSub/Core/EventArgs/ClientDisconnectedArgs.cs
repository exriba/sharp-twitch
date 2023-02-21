using System.Net.WebSockets;

namespace SharpTwitch.EventSub.Core.EventArgs
{
    public class ClientDisconnectedArgs : System.EventArgs
    {
        public string CloseStatusDescription { get; internal set; } = string.Empty;
        public WebSocketCloseStatus WebSocketCloseStatus { get; internal set; }
    }
}
