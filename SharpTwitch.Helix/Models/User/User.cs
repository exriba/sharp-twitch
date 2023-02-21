using SharpTwitch.Core.Enums;
using System.Text.Json.Serialization;

namespace SharpTwitch.Helix.Models.User
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string BroadcasterType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string OfflineImageUrl { get; set; } = string.Empty;
        public long ViewCount { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public BroadcasterType UserBroadcasterType => string.IsNullOrWhiteSpace(Type) ? 
            Core.Enums.BroadcasterType.NORMAL : Enum.Parse<BroadcasterType>(BroadcasterType, true);
    }
}
