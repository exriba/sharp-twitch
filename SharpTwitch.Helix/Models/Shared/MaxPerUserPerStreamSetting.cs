using Newtonsoft.Json;

namespace SharpTwitch.Helix.Models.Shared
{
    public class MaxPerUserPerStreamSetting : StreamSetting
    {
        [JsonProperty(PropertyName = "max_per_user_per_stream")]
        public int MaxPerUserPerStream { get; set; }
    }
}
