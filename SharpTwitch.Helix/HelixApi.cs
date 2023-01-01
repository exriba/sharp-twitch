using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Helix
{
    public sealed class HelixApi
    {
        public readonly ChannelPoints ChannelPoints;

        public HelixApi(ICoreSettings coreSettings, IApiCore apiCore)
        {
            this.ChannelPoints = new ChannelPoints(coreSettings, apiCore);
        }
    }
}