using SharpTwitch.Helix.Models.User;

namespace SharpTwitch.Helix.Users
{
    /// <summary>
    /// Defines Twitch User endpoints.
    /// </summary>
    public interface IUsers
    {
        /// <summary>
        /// Retrieves a collection of users.
        /// </summary>
        /// <param name="userIds">ids of the users</param>
        /// <param name="authCode">authorization code</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A collection of users</returns>
        Task<IEnumerable<User>> GetUsersAsync(string[] userIds, string authCode, CancellationToken cancellationToken);
    }
}
