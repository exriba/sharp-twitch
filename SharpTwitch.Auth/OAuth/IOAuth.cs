using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth.OAuth
{
    /// <summary>
    /// Defines Twitch OAuth endpoints. 
    /// </summary>
    public interface IOAuth
    {
        /// <summary>
        /// Refreshes an access token.
        /// </summary>
        /// <param name="refreshToken">refresh access token</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns><see cref="RefreshTokenResponse"/></returns>
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
