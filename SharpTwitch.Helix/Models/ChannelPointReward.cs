using Newtonsoft.Json;
using SharpTwitch.Helix.Models.Shared;

namespace SharpTwitch.Helix.Models
{
    public class ChannelPointReward
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "broadcaster_name")]
        public string BroadcasterName { get; set; }

        [JsonProperty(PropertyName = "broadcaster_login")]
        public string BroadcasterLogin { get; set; }

        [JsonProperty(PropertyName = "broadcaster_id")]
        public string BroadcasterId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [JsonProperty(PropertyName = "background_color")]
        public string BackgroundColor { get; set; }

        [JsonProperty(PropertyName = "is_enabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Cost { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; }

        [JsonProperty(PropertyName = "is_user_input_required")]
        public bool IsUserInputRequired { get; set; }

        [JsonProperty(PropertyName = "max_per_stream_setting")]
        public MaxPerStreamSetting MaxPerStreamSetting { get; set; }

        [JsonProperty(PropertyName = "max_per_user_per_stream_setting")]
        public MaxPerUserPerStreamSetting MaxPerUserPerStreamSetting { get; set; }

        [JsonProperty(PropertyName = "global_cooldown_setting")]
        public GlobalCooldownSetting GlobalCooldownSetting { get; set; }

        [JsonProperty(PropertyName = "is_paused")]
        public bool IsPaused { get; set; }

        [JsonProperty(PropertyName = "is_in_stock")]
        public bool IsInStock { get; set; }

        [JsonProperty(PropertyName = "default_image")]
        public Image DefaultImage { get; set; }

        [JsonProperty(PropertyName = "should_redemptions_skip_request_queue")]
        public bool ShouldRedemptionsSkipRequestQueue { get; set; }

        [JsonProperty(PropertyName = "redemptions_redeemed_current_stream")]
        public int? RedemptionsRedeemedCurrentStream { get; set; }

        [JsonProperty(PropertyName = "cooldown_expires_at")]
        public string? CooldownExpiresAt { get; set; }
    }
}
