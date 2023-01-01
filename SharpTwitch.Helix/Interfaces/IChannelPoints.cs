using SharpTwitch.Helix.Models;

namespace SharpTwitch.Helix.Interfaces
{
    public interface IChannelPoints
    {
        Task<IEnumerable<ChannelPointReward>> GetChannelPointRewards(string broadcasterId, string authCode, CancellationToken cancellationToken);
    }
}
