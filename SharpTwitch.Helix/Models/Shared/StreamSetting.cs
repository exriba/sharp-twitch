using Newtonsoft.Json;

namespace SharpTwitch.Helix.Models.Shared
{
    public class StreamSetting
    {
        [JsonProperty(PropertyName = "is_enabled")]
        public bool IsEnabled { get; set; }
    }
}
