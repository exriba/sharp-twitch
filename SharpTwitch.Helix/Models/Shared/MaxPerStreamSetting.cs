using Newtonsoft.Json;

namespace SharpTwitch.Helix.Models.Shared
{
    public class MaxPerStreamSetting : StreamSetting
    {
        [JsonProperty(PropertyName = "max_per_stream")]
        public int MaxPerStream { get; set; }
    }
}
