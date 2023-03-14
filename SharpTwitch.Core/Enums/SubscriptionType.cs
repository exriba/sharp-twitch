namespace SharpTwitch.Core.Enums
{
    /// <summary>
    /// Twitch EventSub <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/">Subscription Types</see>
    /// </summary>
    public enum SubscriptionType
    {
        USER_UPDATE,
        STREAM_ONLINE,
        STREAM_OFFLINE,
        CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_ADD,
        CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE,
        CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REMOVE,
        CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD
    }

    public static class SubscriptionTypeExtensions
    {
        /// <summary>
        /// Converts a subscription type into a string.
        /// </summary>
        /// <param name="subscriptionType">subscription type</param>
        /// <returns>The string value of the subscription type</returns>
        /// <exception cref="ArgumentException">If subscription type is invalid</exception>
        public static string ConvertToString(this SubscriptionType subscriptionType)
        {
            return subscriptionType switch
            {
                SubscriptionType.USER_UPDATE => "user.update",
                SubscriptionType.STREAM_ONLINE => "stream.online",
                SubscriptionType.STREAM_OFFLINE => "stream.offline",
                SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_ADD => "channel.channel_points_custom_reward.add",
                SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_UPDATE => "channel.channel_points_custom_reward.update",
                SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REMOVE => "channel.channel_points_custom_reward.remove",
                SubscriptionType.CHANNEL_CHANNEL_POINTS_CUSTOM_REWARD_REDEMPTION_ADD => "channel.channel_points_custom_reward_redemption.add",
                _ => throw new ArgumentException("Invalid Subscription Type", nameof(subscriptionType)),
            };
        }
    }
}
