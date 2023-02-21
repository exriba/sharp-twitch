using SharpTwitch.Auth.OAuth;
using SharpTwitch.Auth.Models;

namespace SharpTwitch.Auth
{
    public interface IAuthApi : IOAuth
    {
        Task<AccessTokenResponse> GetAccessTokenFromCodeAsync(string authCode, CancellationToken cancellationToken);
        Task<ValidateTokenResponse> ValidateAccessTokenAsync(string accessToken, CancellationToken cancellationToken);
    }
}
