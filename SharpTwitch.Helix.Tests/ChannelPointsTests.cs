using Moq;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Interfaces;
using SharpTwitch.Helix.Models;

namespace SharpTwitch.Helix.Tests
{
    public class ChannelPointsTests
    {
        private readonly ChannelPoints _channelPoints;
        private readonly Mock<IApiCore> _mockApiCore;
        private readonly Mock<ICoreSettings> _mockCoreSettings;

        public ChannelPointsTests()
        {
            _mockApiCore = new Mock<IApiCore>();
            _mockCoreSettings = new Mock<ICoreSettings>();
            _mockCoreSettings.Setup(x => x.ClientId).Returns("ClientId");
            _channelPoints = new ChannelPoints(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData("", "code")]
        [InlineData(" ", "code")]
        [InlineData(null, "code")]
        [InlineData("broadcasterId", "")]
        [InlineData("broadcasterId", " ")]
        [InlineData("broadcasterId", null)]
        public void ChannelPoints_GetChannelPointRewards_Throws_InvalidArgs(string broadcasterId, string code)
        {
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await _channelPoints.GetChannelPointRewards(broadcasterId, code, CancellationToken.None));
        }

        [Fact]
        public async Task ChannelPoints_GetChannelPointRewards()
        {
            var response = new HelixCollectionResponse<ChannelPointReward>
            {
                Data = new List<ChannelPointReward>()
                {
                    new ChannelPointReward()
                }
            };

            _mockApiCore.Setup(x => x.GetAsync<HelixCollectionResponse<ChannelPointReward>>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IDictionary<QueryParameter, string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var data = await _channelPoints.GetChannelPointRewards("id", "code", CancellationToken.None);

            Assert.NotEmpty(data);
        }
    }
}
