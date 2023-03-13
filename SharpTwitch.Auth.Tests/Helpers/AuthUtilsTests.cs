using System.Web;
using SharpTwitch.Core.Enums;
using SharpTwitch.Auth.Helpers;

namespace SharpTwitch.Auth.Tests.Helpers
{
    public class AuthUtilsTests
    {
        private const string EMPTY_STRING = "";
        private const string WHITESPACE_STRING = " ";
        private const string CLIENT_ID = "clientId";
        private const string URI = "http://localhost:1234";

        [Theory]
        [InlineData(null, URI, null)]
        [InlineData(EMPTY_STRING, URI, null)]
        [InlineData(WHITESPACE_STRING, URI, null)]
        [InlineData(CLIENT_ID, null, null)]
        [InlineData(CLIENT_ID, EMPTY_STRING, null)]
        [InlineData(CLIENT_ID, WHITESPACE_STRING, null)]
        [InlineData(CLIENT_ID, URI, null)]
        public void AuthUtils_Throws_InvalidArgs(string clientId, string redirectUri, IEnumerable<Scope> scopes)
        {
            var forceVerify = false;
            var state = Guid.NewGuid().ToString();
            
            Assert.ThrowsAny<ArgumentException>(() => AuthUtils.GenerateAuthorizationUrl(clientId, redirectUri, scopes, state, forceVerify));
        }

        [Fact]
        public void AuthUtils_GenerateAuthorizationUrl()
        {
            var state = Guid.NewGuid().ToString();
            var scopes = new Scope[] { Scope.ANALYTICS_READ_EXTENSIONS }.ToList();

            var authUrl = AuthUtils.GenerateAuthorizationUrl(CLIENT_ID, URI, scopes, state);

            Assert.Contains($"client_id={CLIENT_ID}", authUrl);
            Assert.Contains($"redirect_uri={HttpUtility.UrlEncode(URI)}", authUrl);
            Assert.Contains("scope=analytics:read:extensions", authUrl);
            Assert.Contains($"state={state}", authUrl);
        }
    }
}
