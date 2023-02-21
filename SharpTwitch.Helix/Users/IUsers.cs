using SharpTwitch.Helix.Models.User;

namespace SharpTwitch.Helix.Users
{
    public interface IUsers
    {
        Task<IEnumerable<User>> GetUsersAsync(string[] userIds, string authCode, CancellationToken cancellationToken);
    }
}
