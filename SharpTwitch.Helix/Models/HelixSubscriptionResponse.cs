using SharpTwitch.Core.Models.Response;

namespace SharpTwitch.Helix.Models
{
    public class HelixSubscriptionResponse<T> : IResponse where T : class
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
        public int TotalCost { get; set; }
        public int MaxTotalCost { get; set; }
    }
}
