using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth.OAuth
{
    public interface IOAuth
    {
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
