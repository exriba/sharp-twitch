using Moq;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models.User;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Tests.Common;
using SharpTwitch.Core.Models;

namespace SharpTwitch.Helix.Tests.Subscriptions
{
    public class SubscriptionTests : TestFixture
    {
        private const string SESSION_ID = "sessionId";
        private const string SUBSCRIPTION_ID = "subscriptionId";

        private readonly Helix.Subscriptions.Subscriptions _subscriptions;

        public SubscriptionTests() : base()
        {
            _subscriptions = new Helix.Subscriptions.Subscriptions(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData(null, AUTH_CODE, SUBSCRIPTION_ID)]
        [InlineData(EMPTY_STRING, AUTH_CODE, SUBSCRIPTION_ID)]
        [InlineData(WHITESPACE_STRING, AUTH_CODE, SUBSCRIPTION_ID)]
        [InlineData(BROADCASTER_ID, null, SUBSCRIPTION_ID)]
        [InlineData(BROADCASTER_ID, EMPTY_STRING, SUBSCRIPTION_ID)]
        [InlineData(BROADCASTER_ID, WHITESPACE_STRING, SUBSCRIPTION_ID)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, null)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, EMPTY_STRING)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, WHITESPACE_STRING)]
        public async Task Subscriptions_DeleteEventSubSubscriptionAsync_Throws_InvalidArgs(string broadcasterId, string authCode, string subscriptionId)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await _subscriptions.DeleteEventSubSubscriptionAsync(broadcasterId, authCode, subscriptionId, CancellationToken.None));
        }

        [Fact]
        public async Task Subscriptions_DeleteEventSubSubscriptionAsync()
        {
            _mockApiCore.Setup(x => x.DeleteAsync(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IEnumerable<KeyValuePair<QueryParameter, string>>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _subscriptions.DeleteEventSubSubscriptionAsync(BROADCASTER_ID, AUTH_CODE, SUBSCRIPTION_ID, CancellationToken.None);
        }

        [Theory]
        [InlineData(null, AUTH_CODE)]
        [InlineData(EMPTY_STRING, AUTH_CODE)]
        [InlineData(WHITESPACE_STRING, AUTH_CODE)]
        [InlineData(BROADCASTER_ID, null)]
        [InlineData(BROADCASTER_ID, EMPTY_STRING)]
        [InlineData(BROADCASTER_ID, WHITESPACE_STRING)]
        public async Task Subscriptions_GetEventSubSubscriptionAsync_Throws_InvalidArgs(string broadcasterId, string authCode)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await _subscriptions.GetEventSubSubscriptionAsync(broadcasterId, authCode, CancellationToken.None));
        }
        
        [Fact]
        public async Task Subscriptions_GetEventSubSubscriptionAsync()
        {
            var response = new HelixSubscriptionResponse<Subscription>
            {
                Data = new List<Subscription>()
                {
                    new Subscription()
                }
            };

            _mockApiCore.Setup(x => x.GetAsync<HelixSubscriptionResponse<Subscription>>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IEnumerable<KeyValuePair<QueryParameter, string>>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var subscriptions = await _subscriptions.GetEventSubSubscriptionAsync(BROADCASTER_ID, AUTH_CODE, CancellationToken.None);

            Assert.NotEmpty(subscriptions.Data);
        }

        [Theory]
        [InlineData(null, AUTH_CODE, SESSION_ID)]
        [InlineData(EMPTY_STRING, AUTH_CODE, SESSION_ID)]
        [InlineData(WHITESPACE_STRING, AUTH_CODE, SESSION_ID)]
        [InlineData(BROADCASTER_ID, null, SESSION_ID)]
        [InlineData(BROADCASTER_ID, EMPTY_STRING ,SESSION_ID)]
        [InlineData(BROADCASTER_ID, WHITESPACE_STRING, SESSION_ID)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, null)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, EMPTY_STRING)]
        [InlineData(BROADCASTER_ID, AUTH_CODE, WHITESPACE_STRING)]
        public async Task Subscriptions_CreateEventSubSubscriptionAsync_Throws_InvalidArgs(string broadcasterId, string authCode, string sessionId)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await _subscriptions.CreateEventSubSubscriptionAsync(broadcasterId, authCode, sessionId, SubscriptionType.STREAM_ONLINE, CancellationToken.None));
        }

        [Fact]
        public async Task Subscriptions_CreateEventSubSubscriptionAsync()
        {
            var response = new HelixSubscriptionResponse<Subscription>
            {
                Data = new List<Subscription>()
                {
                    new Subscription()
                }
            };

            _mockApiCore.Setup(x => x.PostAsync<HelixSubscriptionResponse<Subscription>>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<StringContent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var subscriptions = await _subscriptions.CreateEventSubSubscriptionAsync(BROADCASTER_ID, AUTH_CODE, SESSION_ID, SubscriptionType.STREAM_ONLINE, CancellationToken.None);

            Assert.NotEmpty(subscriptions.Data);
        }
    }
}
