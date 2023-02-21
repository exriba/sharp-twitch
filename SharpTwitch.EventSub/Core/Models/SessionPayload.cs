namespace SharpTwitch.EventSub.Core.Models
{
    public class SessionPayload : IPayload
    {
        public Session Session { get; set; } = new Session();
    }
}
