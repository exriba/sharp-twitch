using Moq;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Models.Channel.Reward;
using SharpTwitch.Helix.Tests.Common;

namespace SharpTwitch.Helix.Tests.Rewards
{
    public class CustomRewardsTests : TestFixture
    {
        private readonly Helix.Rewards.CustomRewards _customRewards;

        public CustomRewardsTests() : base()
        {
            _customRewards = new Helix.Rewards.CustomRewards(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData(null, AUTH_CODE)]
        [InlineData(EMPTY_STRING, AUTH_CODE)]
        [InlineData(WHITESPACE_STRING, AUTH_CODE)]
        [InlineData(BROADCASTER_ID, null)]
        [InlineData(BROADCASTER_ID, EMPTY_STRING)]
        [InlineData(BROADCASTER_ID, WHITESPACE_STRING)]
        public async Task CustomRewards_GetCustomRewardsAsync_Throws_InvalidArgs(string broadcasterId, string authCode)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await _customRewards.GetCustomRewardsAsync(broadcasterId, authCode, CancellationToken.None));
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
                    It.IsAny<IEnumerable<KeyValuePair<QueryParameter, string>>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var data = await _customRewards.GetCustomRewardsAsync(BROADCASTER_ID, AUTH_CODE, CancellationToken.None);

            Assert.NotEmpty(data);
        }
    }
}
