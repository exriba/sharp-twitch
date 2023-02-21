using Moq;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;

namespace SharpTwitch.Tests
{
    public class TwitchApiTests
    {
        private readonly TwitchAPI _twitchAPI;

        public TwitchApiTests()
        {
            var mockApiCore = new Mock<IApiCore>();
            var mockAppSettings = new Mock<ICoreSettings>();
            _twitchAPI = new TwitchAPI(mockAppSettings.Object, mockApiCore.Object);
        }

        [Fact]
        public void TwitchAPI_Create()
        {
            var auth = _twitchAPI.AuthApi;
            var helix = _twitchAPI.HelixApi;

            Assert.NotNull(auth);
            Assert.NotNull(helix);
        }
    }
}