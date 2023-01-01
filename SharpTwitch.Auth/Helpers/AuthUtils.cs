using System.Web;
using Ardalis.GuardClauses;
using SharpTwitch.Core.Enums;

namespace SharpTwitch.Auth.Helpers
{
    public static class AuthUtils
    {
        private const char SEPARATOR = '+';

        public static string GenerateAuthorizationUrl(string clientId, string redirectUri, IEnumerable<Scope> scopes, string? state = null, bool forceVerify = false)
        {
            Guard.Against.NullOrWhiteSpace(clientId, nameof(clientId));
            Guard.Against.NullOrWhiteSpace(redirectUri, nameof(redirectUri));
            Guard.Against.NullOrEmpty(scopes, nameof(scopes));

            var encodedRedirectUrl = HttpUtility.UrlEncode(redirectUri);
            var scope = string.Join(SEPARATOR, scopes.Select(scope => scope.ConvertToString()));
            return $"https://id.twitch.tv/oauth2/authorize?client_id={clientId}&redirect_uri={encodedRedirectUrl}&response_type=code&scope={scope}&force_verify={forceVerify}&state={state}";
        }
    }
}
