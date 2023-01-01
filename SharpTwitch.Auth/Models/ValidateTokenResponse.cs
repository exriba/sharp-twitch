using Newtonsoft.Json;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Auth.Models
{
    public class ValidateTokenResponse : IResponse
    {
        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; private set; }

        [JsonProperty(PropertyName = "login")]
        public string Login { get; private set; }

        [JsonProperty(PropertyName = "scopes")]
        public List<string> Scopes { get; private set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; private set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; private set; }
    }
}
