using SharpTwitch.Core;
using SharpTwitch.Core.Settings;

namespace SharpTwitch.Helix
{
    public sealed class HelixApi
    {
        public readonly Users.Users Users;
        public readonly Rewards.CustomRewards CustomRewards;
        public readonly Subscriptions.Subscriptions Subscriptions;

        public HelixApi(ICoreSettings coreSettings, IApiCore apiCore)
        {
            this.Users = new Users.Users(coreSettings, apiCore);
            this.CustomRewards = new Rewards.CustomRewards(coreSettings, apiCore);
            this.Subscriptions = new Subscriptions.Subscriptions(coreSettings, apiCore);
        }
    }
}