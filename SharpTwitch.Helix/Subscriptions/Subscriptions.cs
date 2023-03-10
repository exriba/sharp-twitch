using SharpTwitch.Core.Settings;
using SharpTwitch.Core;
using Ardalis.GuardClauses;
using SharpTwitch.Core.Enums;
using SharpTwitch.Helix.Models;
using System.Text.Json;
using System.Text;
using SharpTwitch.Core.Models;

namespace SharpTwitch.Helix.Subscriptions
{
    public class Subscriptions : ISubscriptions
    {
        private readonly IApiCore _apiCore;
        private readonly ICoreSettings _coreSettings;

        public Subscriptions(ICoreSettings coreSettings, IApiCore apiCore)
        {
            _apiCore = apiCore;
            _coreSettings = coreSettings;
        }

        public async Task DeleteEventSubSubscriptionAsync(string broadcasterId, string authCode, string subscriptionId, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(subscriptionId, nameof(subscriptionId));
            Guard.Against.NullOrWhiteSpace(broadcasterId, nameof(broadcasterId));
            Guard.Against.NullOrWhiteSpace(authCode, nameof(authCode));

            var headers = new Dictionary<Header, string>
            {
                { Header.AUTHORIZATION_BEARER, authCode },
                { Header.CLIENT_ID, _coreSettings.ClientId },
            };

            var queryParams = new Dictionary<QueryParameter, string>
            {
                { QueryParameter.ID, subscriptionId },
            };

            await _apiCore.DeleteAsync(UrlFragment.HELIX_EVENTSUB_SUBSCRIPTION, headers, queryParams, cancellationToken)
                          .ConfigureAwait(false);
        }

        public async Task<HelixSubscriptionResponse<Subscription>> GetEventSubSubscriptionAsync(string broadcasterId, string authCode, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(broadcasterId, nameof(broadcasterId));
            Guard.Against.NullOrWhiteSpace(authCode, nameof(authCode));

            var headers = new Dictionary<Header, string>
            {
                { Header.AUTHORIZATION_BEARER, authCode },
                { Header.CLIENT_ID, _coreSettings.ClientId },
            };

            return await _apiCore.GetAsync<HelixSubscriptionResponse<Subscription>>(UrlFragment.HELIX_EVENTSUB_SUBSCRIPTION, headers, null, cancellationToken)
                                 .ConfigureAwait(false);
        }

        public async Task<HelixSubscriptionResponse<Subscription>> CreateEventSubSubscriptionAsync(string broadcasterId, string authCode, string sessionId, SubscriptionType subscriptionType, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrWhiteSpace(broadcasterId, nameof(broadcasterId));
            Guard.Against.NullOrEmpty(sessionId, nameof(sessionId));

            var headers = new Dictionary<Header, string>
            {
                { Header.CLIENT_ID, _coreSettings.ClientId },
            };

            if (!string.IsNullOrEmpty(authCode))
                headers.Add(Header.AUTHORIZATION_BEARER, authCode);

            var requestBody = new SubscriptionRequest
            {
                Type = subscriptionType.ConvertToString(),
                Condition = new Condition
                {
                    UserId = broadcasterId,
                    BroadcasterUserId = broadcasterId,
                },
                Transport = new Transport
                {
                    Method = TransportMethod.WEBSOCKET.ConvertToString(),
                    SessionId = sessionId,
                }
            };

            var requestBodyString = JsonSerializer.Serialize(requestBody, _apiCore.JsonSerializerOptions);
            var content = new StringContent(requestBodyString, Encoding.UTF8, Header.CONTENT_TYPE_APPLICATION_JSON.ConvertToString());

            return await _apiCore.PostAsync<HelixSubscriptionResponse<Subscription>>(UrlFragment.HELIX_EVENTSUB_SUBSCRIPTION, headers, content, cancellationToken)
                                 .ConfigureAwait(false);
        }
    }
}
