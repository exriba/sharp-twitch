using SharpTwitch.Core.Models.Response;

namespace SharpTwitch.Helix.Models
{
    public class HelixCollectionResponse<T> : IResponse where T : class
    {
        public IList<T> Data { get; set; }
    }
}
