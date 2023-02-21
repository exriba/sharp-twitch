namespace SharpTwitch.EventSub.Core.Models.User
{
    /// <summary>
    /// https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userupdate
    /// </summary>
    public class UserUpdate
    {
        public string UserId { get; set; } = string.Empty;
        public string UserLogin { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
