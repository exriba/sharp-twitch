using SharpTwitch.Auth.OAuth;
using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth
{
    /// <inheritdoc/>
    public interface IAuthApi : IOAuth
    {
        /// <summary>
        /// Retrieves an access token from an authorization code.
        /// </summary>
        /// <param name="authCode">authorization code</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns><see cref="AccessTokenResponse"/></returns>
        Task<AccessTokenResponse> GetAccessTokenFromCodeAsync(string authCode, CancellationToken cancellationToken);

        /// <summary>
        /// Validates an access token.
        /// </summary>
        /// <param name="accessToken">access token</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns><see cref="ValidateTokenResponse"/></returns>
        Task<ValidateTokenResponse> ValidateAccessTokenAsync(string accessToken, CancellationToken cancellationToken);
    }
}
