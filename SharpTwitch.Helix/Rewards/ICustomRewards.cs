using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.Helix.Rewards
{
    public interface ICustomRewards
    {
        Task<IEnumerable<CustomReward>> GetCustomRewardsAsync(string broadcasterId, string authCode, CancellationToken cancellationToken);
    }
}
