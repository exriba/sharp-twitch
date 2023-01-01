using Ardalis.GuardClauses;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Interfaces;
using SharpTwitch.Helix.Interfaces;
using SharpTwitch.Helix.Models;

namespace SharpTwitch.Helix
{
    public class ChannelPoints : IChannelPoints
    {
        private readonly IApiCore _apiCore;
        private readonly ICoreSettings _coreSettings;

        public ChannelPoints(ICoreSettings coreSettings, IApiCore apiCore)
        {
            _apiCore = apiCore;
            _coreSettings = coreSettings;
        }

        public async Task<IEnumerable<ChannelPointReward>> GetChannelPointRewards(string broadcasterId, string authCode, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(broadcasterId, nameof(broadcasterId));
            Guard.Against.NullOrWhiteSpace(authCode, nameof(authCode));

            var headers = new Dictionary<Header, string>
            {
                { Header.AUTHORIZATION_BEARER, authCode },
                { Header.CLIENT_ID, _coreSettings.ClientId },
            };

            var queryParams = new Dictionary<QueryParameter, string>
            {
                { QueryParameter.BROADCASTER_ID, broadcasterId },
            };

            var response = await _apiCore.GetAsync<HelixCollectionResponse<ChannelPointReward>>(UrlFragment.HELIX_CUSTOM_REWARDS, headers, queryParams, cancellationToken);
            return response.Data;
        }
    }
}
