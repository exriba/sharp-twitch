namespace SharpTwitch.Auth.Models
{
    public sealed class AccessTokenResponse : TokenResponseBase
    {
        public int ExpiresIn { get; set; }
    }
}
