using Ardalis.GuardClauses;
using SharpTwitch.Core.Enums;
using System.Web;

namespace SharpTwitch.Auth.Helpers
{
    /// <summary>
    /// Utility class for Authorization purposes.
    /// </summary>
    public static class AuthUtils
    {
        private const char SEPARATOR = '+';
        private const string TWITCH_AUTHORIZATION_URL_BASE = "https://id.twitch.tv/oauth2/authorize";

        /// <summary>
        /// Generates an authorization url following Twitch <see href="https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#authorization-code-grant-flow">Authorization code grant flow</see>
        /// </summary>
        /// <param name="clientId">your app’s registered client ID</param>
        /// <param name="redirectUri">your app’s registered redirect URI. The authorization code is sent to this URI</param>
        /// <param name="scopes">A list of scopes</param>
        /// <param name="state">a state string to help prevent Cross-Site Request Forgery (CSRF) attacks (Optional)</param>
        /// <param name="forceVerify">set to true to force the user to re-authorize your app’s access to their resources</param>
        /// <returns>Url string</returns>
        public static string GenerateAuthorizationUrl(string clientId, string redirectUri, IEnumerable<Scope> scopes, string? state = null, bool forceVerify = false)
        {
            Guard.Against.NullOrWhiteSpace(clientId, nameof(clientId));
            Guard.Against.NullOrWhiteSpace(redirectUri, nameof(redirectUri));
            Guard.Against.NullOrEmpty(scopes, nameof(scopes));

            var encodedRedirectUrl = HttpUtility.UrlEncode(redirectUri);
            var scope = string.Join(SEPARATOR, scopes.Select(scope => scope.ConvertToString()));
            return $"{TWITCH_AUTHORIZATION_URL_BASE}?client_id={clientId}&redirect_uri={encodedRedirectUrl}&response_type=code&scope={scope}&force_verify={forceVerify}&state={state}";
        }
    }
}
