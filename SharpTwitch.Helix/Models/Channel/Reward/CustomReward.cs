using SharpTwitch.Helix.Models.Shared;

namespace SharpTwitch.Helix.Models.Channel.Reward
{
    public class CustomReward
    {
        public string Id { get; set; } = string.Empty;
        public string BroadcasterName { get; set; } = string.Empty;
        public string BroadcasterLogin { get; set; } = string.Empty;
        public string BroadcasterUserId { get; set; } = string.Empty;
        public Image Image { get; set; } = new Image();
        public string BackgroundColor { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public int Cost { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public bool IsUserInputRequired { get; set; }
        public MaxPerStreamSetting MaxPerStreamSetting { get; set; } = new MaxPerStreamSetting();
        public MaxPerUserPerStreamSetting MaxPerUserPerStreamSetting { get; set; } = new MaxPerUserPerStreamSetting();
        public GlobalCooldownSetting GlobalCooldownSetting { get; set; } = new GlobalCooldownSetting();
        public bool IsPaused { get; set; }
        public bool IsInStock { get; set; }
        public Image DefaultImage { get; set; } = new Image();
        public bool ShouldRedemptionsSkipRequestQueue { get; set; }
        public int? RedemptionsRedeemedCurrentStream { get; set; }
        public string? CooldownExpiresAt { get; set; }
    }
}
