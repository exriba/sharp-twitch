namespace SharpTwitch.EventSub.Core.Models
{
    public class EventSubMessage<T> where T : IPayload
    {
        public Metadata Metadata { get; set; } = new Metadata();
        public T Payload { get; set; }
    }
}
