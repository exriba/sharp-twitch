using SharpTwitch.Core.Interfaces;

namespace SharpTwitch
{
    public sealed class TwitchAPI
    {
        public readonly Auth.AuthApi Auth;
        public readonly Helix.HelixApi Helix;

        public TwitchAPI(ICoreSettings coreSettings, IApiCore apiCore)
        {
            Auth = new Auth.AuthApi(coreSettings, apiCore);
            Helix = new Helix.HelixApi(coreSettings, apiCore);
        }
    }
}
