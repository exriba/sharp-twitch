using Ardalis.GuardClauses;
using SharpTwitch.Core;
using SharpTwitch.Core.Settings;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Models.Channel.Reward;

namespace SharpTwitch.Helix.Rewards
{
    /// <summary>
    /// Default implementation of ICustomRewards.
    /// </summary>
    public class CustomRewards : ICustomRewards
    {
        private readonly IApiCore _apiCore;
        private readonly ICoreSettings _coreSettings;

        public CustomRewards(ICoreSettings coreSettings, IApiCore apiCore)
        {
            _apiCore = apiCore;
            _coreSettings = coreSettings;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If broadcasterId or authorization code is null</exception>
        /// <exception cref="ArgumentException">If broadcasterId or authorization code is empty or whitespace string</exception>
        public async Task<IEnumerable<CustomReward>> GetCustomRewardsAsync(string broadcasterId, string authCode, CancellationToken cancellationToken)
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

            var response = await _apiCore.GetAsync<HelixCollectionResponse<CustomReward>>(UrlFragment.HELIX_CUSTOM_REWARDS, headers, queryParams, cancellationToken)
                                         .ConfigureAwait(false);
            return response.Data;
        }
    }
}
