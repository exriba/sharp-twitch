using Newtonsoft.Json;
using SharpTwitch.Core.Interfaces;

namespace SharpTwitch.Helix.Models
{
    public class HelixCollectionResponse<T> : IResponse
    {
        [JsonProperty(PropertyName = "data")]
        public IList<T> Data { get; set; } = new List<T>();
    }
}
