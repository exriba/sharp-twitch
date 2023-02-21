using SharpTwitch.EventSub.Core.Models.Channel.ChannelPoints;

namespace SharpTwitch.EventSub.Core.SubscriptionTypes.Channel.Redemption
{
    public class ChannelPointsCustomRewardRedemption
    {
        public string Id { get; set; } = string.Empty;
        public string BroadcasterUserId { get; set; } = string.Empty;
        public string BroadcasterUserName { get; set; } = string.Empty;
        public string BroadcasterUserLogin { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserLogin { get; set; } = string.Empty;
        public string UserInput { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public RedemptionReward Reward { get; set; } = new RedemptionReward();
        public DateTimeOffset RedeemedAt { get; set; }
    }
}
