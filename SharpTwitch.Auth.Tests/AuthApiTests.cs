using Moq;
using SharpTwitch.Auth.Models;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;
using SharpTwitch.Core.Enums;

namespace SharpTwitch.Auth.Tests
{
    public class AuthApiTests
    {
        private readonly AuthApi _authApi;
        private readonly Mock<IApiCore> _mockApiCore;
        private readonly Mock<ICoreSettings> _mockCoreSettings;

        public AuthApiTests() 
        {
            _mockApiCore = new Mock<IApiCore>();
            _mockCoreSettings = new Mock<ICoreSettings>();
            _mockCoreSettings.Setup(x => x.Secret).Returns("Secret");
            _mockCoreSettings.Setup(x => x.ClientId).Returns("ClientId");
            _mockCoreSettings.Setup(x => x.RedirectUri).Returns("RedirectUri");
            _authApi = new AuthApi(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AuthApi_GetAccessTokenFromCodeAsync_Throws_InvalidArgs(string code)
        {
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await _authApi.GetAccessTokenFromCodeAsync(code, CancellationToken.None));
        }

        [Fact]
        public async Task AuthApi_GetAccessTokenFromCodeAsync()
        {
            var tokenResponse = new AccessTokenResponse();

            _mockApiCore.Setup(x => x.PostAsync<AccessTokenResponse>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<FormUrlEncodedContent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tokenResponse));

            var data = await _authApi.GetAccessTokenFromCodeAsync("code", CancellationToken.None);

            Assert.Equal(tokenResponse, data);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AuthApi_RefreshAccessTokenAsync_Throws_InvalidArgs(string token)
        {
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await _authApi.RefreshAccessTokenAsync(token, CancellationToken.None));
        }

        [Fact]
        public async Task AuthApi_RefreshAccessTokenAsync()
        {
            var tokenResponse = new RefreshTokenResponse();

            _mockApiCore.Setup(x => x.PostAsync<RefreshTokenResponse>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<FormUrlEncodedContent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tokenResponse));

            var data = await _authApi.RefreshAccessTokenAsync("token", CancellationToken.None);

            Assert.Equal(tokenResponse, data);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AuthApi_ValidateAccessTokenAsync_Throws_InvalidArgs(string token)
        {
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await _authApi.ValidateAccessTokenAsync(token, CancellationToken.None));
        }

        [Fact]
        public async Task AuthApi_ValidateAccessTokenAsync()
        {
            var tokenResponse = new ValidateTokenResponse();

            _mockApiCore.Setup(x => x.GetAsync<ValidateTokenResponse>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IDictionary<QueryParameter, string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tokenResponse));

            var data = await _authApi.ValidateAccessTokenAsync("token", CancellationToken.None);

            Assert.Equal(tokenResponse, data);
        }
    }
}
