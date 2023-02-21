using SharpTwitch.Core.Models.Response;

namespace SharpTwitch.Auth.Models
{
    public abstract class TokenResponseBase : IResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string[] Scope { get; set; } = Array.Empty<string>();
        public string TokenType { get; set; } = string.Empty;
    }
}
