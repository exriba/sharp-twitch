using Ardalis.GuardClauses;
using SharpTwitch.Core;
using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Settings;
using SharpTwitch.Helix.Models;
using SharpTwitch.Helix.Models.User;

namespace SharpTwitch.Helix.Users
{
    /// <summary>
    /// Default implementation of IUsers.
    /// </summary>
    public class Users : IUsers
    {
        private readonly IApiCore _apiCore;
        private readonly ICoreSettings _coreSettings;

        public Users(ICoreSettings coreSettings, IApiCore apiCore)
        {
            _apiCore = apiCore;
            _coreSettings = coreSettings;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If the authorization code is null</exception>
        /// <exception cref="ArgumentException">If the authorization code is empty or whitespace string</exception>
        public async Task<IEnumerable<User>> GetUsersAsync(string[] userIds, string authCode, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(authCode, nameof(authCode));

            var headers = new Dictionary<Header, string>
            {
                { Header.AUTHORIZATION_BEARER, authCode },
                { Header.CLIENT_ID, _coreSettings.ClientId },
            };

            List<KeyValuePair<QueryParameter, string>>? queryParams = null;

            if (userIds is not null && userIds.Length > 0)
            {
                queryParams = new List<KeyValuePair<QueryParameter, string>>();

                foreach (var userId in userIds)
                    queryParams.Add(new KeyValuePair<QueryParameter, string>(QueryParameter.ID, userId));
            }

            var response = await _apiCore.GetAsync<HelixCollectionResponse<User>>(UrlFragment.HELIX_USER, headers, queryParams, cancellationToken)
                                         .ConfigureAwait(false);
            return response.Data;
        }
    }
}
