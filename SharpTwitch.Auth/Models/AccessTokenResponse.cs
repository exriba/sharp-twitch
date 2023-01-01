using Newtonsoft.Json;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Auth.Models
{
    public sealed class AccessTokenResponse : IResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; private set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; private set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; private set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; private set; }

        [JsonProperty(PropertyName = "scope")]
        public string[] Scopes { get; private set; }
    }
}
