using Moq;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.Helix.Tests
{
    public class CustomRewardsTests
    {
        private readonly Rewards.CustomRewards _customRewards;
        private readonly Mock<IApiCore> _mockApiCore;
        private readonly Mock<ICoreSettings> _mockCoreSettings;

        public CustomRewardsTests()
        {
            _mockApiCore = new Mock<IApiCore>();
            _mockCoreSettings = new Mock<ICoreSettings>();
            _mockCoreSettings.Setup(x => x.ClientId).Returns("ClientId");
            _customRewards = new Rewards.CustomRewards(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData("", "code")]
        [InlineData(" ", "code")]
        [InlineData(null, "code")]
        [InlineData("broadcasterId", "")]
        [InlineData("broadcasterId", " ")]
        [InlineData("broadcasterId", null)]
        public void CustomRewards_GetCustomRewardsAsync_Throws_InvalidArgs(string broadcasterId, string code)
        {
            Assert.ThrowsAnyAsync<ArgumentException>(async () => await _customRewards.GetCustomRewardsAsync(broadcasterId, code, CancellationToken.None));
        }

        [Fact]
        public async Task CustomRewards_GetCustomRewardsAsync()
        {
            var response = new HelixCollectionResponse<CustomReward>
            {
                Data = new List<CustomReward>()
                {
                    new CustomReward()
                }
            };

            _mockApiCore.Setup(x => x.GetAsync<HelixCollectionResponse<CustomReward>>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IDictionary<QueryParameter, string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var data = await _customRewards.GetCustomRewardsAsync("id", "code", CancellationToken.None);

            Assert.NotEmpty(data);
        }
    }
}
