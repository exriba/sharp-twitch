using SharpTwitch.Core.Enums;
using System.Text.Json.Serialization;

namespace SharpTwitch.Core.Models
{
    public class Transport
    {
        public string Method { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DisconnectedAt { get; set; }

        [JsonIgnore]
        public TransportMethod TransportMethod => Enum.Parse<TransportMethod>(Method, true);
    }
}
