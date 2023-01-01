namespace SharpTwitch.Core.Enums
{
    public enum UrlFragment
    {
        HELIX_USER,
        HELIX_CUSTOM_REWARDS,
        OAUTH2_TOKEN,
        OAUTH2_TOKEN_VALIDATION
    }

    public static class UrlFragmentExtensions
    {
        public static string ConvertToString(this UrlFragment fragment)
        {
            return fragment switch
            {
                UrlFragment.HELIX_USER => "https://api.twitch.tv/helix/users",
                UrlFragment.HELIX_CUSTOM_REWARDS => "https://api.twitch.tv/helix/channel_points/custom_rewards",
                UrlFragment.OAUTH2_TOKEN => "https://id.twitch.tv/oauth2/token",
                UrlFragment.OAUTH2_TOKEN_VALIDATION => "https://id.twitch.tv/oauth2/validate",
                _ => throw new ArgumentException("Invalid Url Fragment", nameof(fragment)),
            };
        }
    }
}
