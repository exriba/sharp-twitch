using Moq;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;

namespace SharpTwitch.Helix.Tests.Common
{
    public abstract class TestFixture
    {
        protected const string EMPTY_STRING = "";
        protected const string AUTH_CODE = "authCode";
        protected const string WHITESPACE_STRING = " ";
        protected const string BROADCASTER_ID = "broadcasterId";

        protected readonly Mock<IApiCore> _mockApiCore;
        protected readonly Mock<ICoreSettings> _mockCoreSettings;

        protected TestFixture()
        {
            _mockApiCore = new Mock<IApiCore>();
            _mockCoreSettings = new Mock<ICoreSettings>();
            _mockCoreSettings.Setup(x => x.ClientId).Returns("ClientId");
        }
    }
}
