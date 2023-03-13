using Moq;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Tests.Common;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models.User;

namespace SharpTwitch.Helix.Tests.Users
{
    public class UsersTests : TestFixture
    {
        private readonly Helix.Users.Users _users;

        public UsersTests() : base()
        {
            _users = new Helix.Users.Users(_mockCoreSettings.Object, _mockApiCore.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(EMPTY_STRING)]
        [InlineData(WHITESPACE_STRING)]
        public async Task Users_GetUsersAsync_Throws_InvalidArgs(string authCode)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await _users.GetUsersAsync(Array.Empty<string>(), authCode, CancellationToken.None));
        }

        [Fact]
        public async Task Users_GetUsersAsync()
        {
            var response = new HelixCollectionResponse<User>
            {
                Data = new List<User>()
                {
                    new User()
                }
            };

            _mockApiCore.Setup(x => x.GetAsync<HelixCollectionResponse<User>>(
                    It.IsAny<UrlFragment>(),
                    It.IsAny<IDictionary<Header, string>>(),
                    It.IsAny<IEnumerable<KeyValuePair<QueryParameter, string>>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var data = await _users.GetUsersAsync(new string[] { BROADCASTER_ID }, AUTH_CODE, CancellationToken.None);

            Assert.NotEmpty(data);
        }
    }
}
