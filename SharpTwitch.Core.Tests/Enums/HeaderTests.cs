using SharpTwitch.Core.Enums;

namespace SharpTwitch.Core.Tests.Enums
{
    public class HeaderTests
    {
        private const string CONTENT = "1234";

        [Theory]
        [InlineData(Header.CODE)]
        [InlineData(Header.GRANT_TYPE)]
        [InlineData(Header.REDIRECT_URI)]
        [InlineData(Header.REFRESH_TOKEN)]
        [InlineData(Header.CLIENT_SECRET)]
        public void Header_Transform_Throws(Header header)
        {
            Assert.Throws<NotImplementedException>(() => header.Transform(CONTENT));
        }

        [Theory]
        [InlineData(Header.CLIENT_ID, "Client-Id")]
        [InlineData(Header.AUTHORIZATION_BEARER, "Authorization")]
        [InlineData(Header.AUTHORIZATION_ACCESS_TOKEN, "Authorization")]
        [InlineData(Header.CONTENT_TYPE_FORM_URL_ENCODED, "Content-Type")]
        [InlineData(Header.CONTENT_TYPE_APPLICATION_JSON, "Content-Type")]
        public void Header_Transform(Header header, string key)
        {
            var kvp = header.Transform(CONTENT);
            
            Assert.Equal(kvp.Key, key);
            Assert.NotNull(kvp.Value);
        }
    }
}
