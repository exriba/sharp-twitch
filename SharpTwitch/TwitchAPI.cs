using SharpTwitch.Auth;
using SharpTwitch.Helix;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;

namespace SharpTwitch
{
    public sealed class TwitchAPI
    {
        public readonly AuthApi AuthApi;
        public readonly HelixApi HelixApi;

        public TwitchAPI(ICoreSettings coreSettings, IApiCore apiCore)
        {
            AuthApi = new AuthApi(coreSettings, apiCore);
            HelixApi = new HelixApi(coreSettings, apiCore);
        }
    }
}
