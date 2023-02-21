using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Models;
using SharpTwitch.Helix.Models;

namespace SharpTwitch.Helix.Subscriptions
{
    public interface ISubscriptions
    {
        Task DeleteEventSubSubscriptionAsync(string broadcasterId, string authCode, string subscriptionId, CancellationToken cancellationToken);
        Task<HelixSubscriptionResponse<Subscription>> GetEventSubSubscriptionAsync(string broadcasterId, string authCode, CancellationToken cancellationToken);
        Task<HelixSubscriptionResponse<Subscription>> CreateEventSubSubscriptionAsync(string broadcasterId, string authCode, string sessionId, SubscriptionType subscriptionType, CancellationToken cancellationToken);
    }
}
