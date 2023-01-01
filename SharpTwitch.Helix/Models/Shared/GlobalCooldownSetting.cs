using Newtonsoft.Json;

namespace SharpTwitch.Helix.Models.Shared
{
    public class GlobalCooldownSetting : StreamSetting
    {
        [JsonProperty(PropertyName = "global_cooldown_seconds")]
        public int GlobalCooldownSeconds { get; set; }
    }
}
