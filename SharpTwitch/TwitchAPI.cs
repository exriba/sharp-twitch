using SharpTwitch.Auth;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;
using SharpTwitch.Helix;

namespace SharpTwitch
{
    /// <summary>
    /// Wrapper for AuthApi and HelixApi components.
    /// </summary>
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
