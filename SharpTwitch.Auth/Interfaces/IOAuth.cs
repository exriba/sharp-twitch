using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth.Interfaces
{
    public interface IOAuth
    {
        Task<RefreshTokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
