using Ardalis.GuardClauses;
using SharpTwitch.Auth.Models;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;
using SharpTwitch.Core.Enums;

namespace SharpTwitch.Auth
{
    public sealed class AuthApi : IAuthApi
    {
        private readonly IApiCore _apiCore;
        private readonly ICoreSettings _coreSettings;

        public AuthApi(ICoreSettings coreSettings, IApiCore apiCore)
        {
            _apiCore = apiCore;
            _coreSettings = coreSettings;
        }

        public async Task<AccessTokenResponse> GetAccessTokenFromCodeAsync(string authCode, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(authCode, nameof(authCode));

            var headers = new Dictionary<Header, string>
            {
                { Header.CONTENT_TYPE_FORM_URL_ENCODED, string.Empty },
            };

            var content = new Dictionary<Header, string>
            {
                { Header.CODE, authCode },
                { Header.CLIENT_ID, _coreSettings.ClientId },
                { Header.REDIRECT_URI, _coreSettings.RedirectUri },
                { Header.CLIENT_SECRET, _coreSettings.Secret },
                { Header.GRANT_TYPE, "authorization_code" }
            };

            var args = content.ToDictionary(kvp => kvp.Key.ToString().ToLower(), kvp => kvp.Value);
            var formUrlEncodedContent = new FormUrlEncodedContent(args);

            return await _apiCore.PostAsync<AccessTokenResponse>(UrlFragment.OAUTH2_TOKEN, headers, formUrlEncodedContent, cancellationToken)
                                 .ConfigureAwait(false);
        }

        public async Task<RefreshTokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(refreshToken, nameof(refreshToken));

            var headers = new Dictionary<Header, string>
            {
                { Header.CONTENT_TYPE_FORM_URL_ENCODED, string.Empty },
            };

            var content = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, _coreSettings.ClientId },
                { Header.REFRESH_TOKEN, refreshToken },
                { Header.CLIENT_SECRET, _coreSettings.Secret },
                { Header.GRANT_TYPE, "refresh_token" }
            };

            var args = content.ToDictionary(kvp => kvp.Key.ToString().ToLower(), kvp => kvp.Value);
            var formUrlEncodedContent = new FormUrlEncodedContent(args);

            return await _apiCore.PostAsync<RefreshTokenResponse>(UrlFragment.OAUTH2_TOKEN, headers, formUrlEncodedContent, cancellationToken)
                                 .ConfigureAwait(false);
        }

        public async Task<ValidateTokenResponse> ValidateAccessTokenAsync(string accessToken, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(accessToken, nameof(accessToken));

            var headers = new Dictionary<Header, string>
            {
                { Header.AUTHORIZATION_ACCESS_TOKEN, accessToken }
            };

            return await _apiCore.GetAsync<ValidateTokenResponse>(UrlFragment.OAUTH2_TOKEN_VALIDATION, headers, null, cancellationToken)
                                 .ConfigureAwait(false);
        }
    }
}
