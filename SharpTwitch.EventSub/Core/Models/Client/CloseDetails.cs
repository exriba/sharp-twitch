using System.Net.WebSockets;

namespace SharpTwitch.EventSub.Core.Models.Client
{
    internal class CloseDetails
    {
        public WebSocketCloseStatus CloseStatus { get; internal set; }
        public string Description { get; internal set; } = string.Empty;
    }
}
