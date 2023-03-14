namespace SharpTwitch.Auth.Models
{
    /// <summary>
    /// Access Token Model.
    /// </summary>
    /// <see/>https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#use-the-authorization-code-to-get-a-token</see>
    public sealed class AccessTokenResponse : TokenResponseBase
    {
        public int ExpiresIn { get; set; }
    }
}
