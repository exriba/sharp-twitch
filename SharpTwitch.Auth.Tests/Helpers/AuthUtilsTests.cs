using System.Web;
using SharpTwitch.Core.Enums;
using SharpTwitch.Auth.Helpers;

namespace SharpTwitch.Auth.Tests.Helpers
{
    public class AuthUtilsTests
    {
        private const string ClientId = "clientId";
        private const string Uri = "http://localhost:1234";

        [Theory]
        [InlineData("", null, null)]
        [InlineData(" ", null, null)]
        [InlineData(null, null, null)]
        [InlineData(ClientId, "", null)]
        [InlineData(ClientId, " ", null)]
        [InlineData(ClientId, null, null)]
        [InlineData(ClientId, Uri, null)]
        public void GenerateAuthorizationUrl_ShouldHandleInvalidArgs(string clientId, string redirectUri, IEnumerable<Scope> scopes)
        {
            var forceVerify = false;
            var state = Guid.NewGuid().ToString();
            Assert.ThrowsAny<ArgumentException>(() => AuthUtils.GenerateAuthorizationUrl(clientId, redirectUri, scopes, state, forceVerify));
        }

        [Fact]
        public void GenerateAuthorizationUrl_ShouldReturnTwitchFormattedAuthenticationUrl()
        {
            var state = Guid.NewGuid().ToString();
            var scopes = new Scope[] { Scope.ANALYTICS_READ_EXTENSIONS }.ToList();

            var authUrl = AuthUtils.GenerateAuthorizationUrl(ClientId, Uri, scopes, state);
            Assert.Contains($"client_id={ClientId}", authUrl);
            Assert.Contains($"redirect_uri={HttpUtility.UrlEncode(Uri)}", authUrl);
            Assert.Contains("scope=analytics:read:extensions", authUrl);
            Assert.Contains($"state={state}", authUrl);
        }
    }
}
