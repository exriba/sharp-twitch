namespace SharpTwitch.Core.Enums
{
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
