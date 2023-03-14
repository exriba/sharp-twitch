using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.Helix.Rewards
{
    /// <summary>
    /// Defines Twitch Custom Reward endpoints.
    /// </summary>
    public interface ICustomRewards
    {
        /// <summary>
        /// Retrieves a collection of custom rewards.
        /// </summary>
        /// <param name="broadcasterId">id of the broadcaster</param>
        /// <param name="authCode">authorization code</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A collection of custom rewards</returns>
        Task<IEnumerable<CustomReward>> GetCustomRewardsAsync(string broadcasterId, string authCode, CancellationToken cancellationToken);
    }
}
