using Moq;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Helix.Tests
{
    public class HelixApiTests
    {
        private readonly HelixApi _helixApi;

        public HelixApiTests()
        {
            var mockApiCore = new Mock<IApiCore>();
            var mockAppSettings = new Mock<ICoreSettings>();
            _helixApi = new HelixApi(mockAppSettings.Object, mockApiCore.Object);
        }

        [Fact]
        public void HelixApi_Create()
        {
            var channelPoints = _helixApi.ChannelPoints;

            Assert.NotNull(channelPoints);
        }
    }
}