using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth.Interfaces
{
    public interface IAuthApi : IOAuth
    {
        Task<AccessTokenResponse> GetAccessTokenFromCodeAsync(string authCode, CancellationToken cancellationToken);
        Task<ValidateTokenResponse> ValidateAccessTokenAsync(string accessToken, CancellationToken cancellationToken);
    }
}
