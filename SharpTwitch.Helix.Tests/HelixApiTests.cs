using Moq;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;

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
        public void HelixApi_Constructor()
        {
            var users = _helixApi.Users;
            var customRewards = _helixApi.CustomRewards;
            var subscriptions = _helixApi.Subscriptions;

            Assert.NotNull(users);
            Assert.NotNull(customRewards);
            Assert.NotNull(subscriptions);
        }
    }
}