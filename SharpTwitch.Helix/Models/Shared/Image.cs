using Newtonsoft.Json;

namespace SharpTwitch.Helix.Models.Shared
{
    public class Image
    {
        [JsonProperty(PropertyName = "url_1x")]
        public string Url1x { get; set; }

        [JsonProperty(PropertyName = "url_2x")]
        public string Url2x { get; set; }

        [JsonProperty(PropertyName = "url_4x")]
        public string Url4x { get; set; }
    }
}
