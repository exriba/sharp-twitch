using SharpTwitch.Core.Models.Response;

namespace SharpTwitch.Auth.Models
{
    /// <summary>
    /// Validate Token Model.
    /// </summary>
    /// <see/>https://dev.twitch.tv/docs/authentication/validate-tokens/#how-to-validate-a-token</see>
    public class ValidateTokenResponse : IResponse
    {
        public string ClientId { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string[] Scopes { get; set; } = Array.Empty<string>();
        public string UserId { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
