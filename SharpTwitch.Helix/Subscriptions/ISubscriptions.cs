using SharpTwitch.Core.Enums;
using SharpTwitch.Core.Models;
using SharpTwitch.Helix.Models;

namespace SharpTwitch.Helix.Subscriptions
{
    /// <summary>
    /// Defines Twitch EventSub Subscription endpoints.
    /// </summary>
    public interface ISubscriptions
    {
        /// <summary>
        /// Deletes an event sub subscription.
        /// </summary>
        /// <param name="broadcasterId">id of the broadcaster</param>
        /// <param name="authCode">authorization code</param>
        /// <param name="subscriptionId">id of the subscription</param>
        /// <param name="cancellationToken">cancellation token</param>
        Task DeleteEventSubSubscriptionAsync(string broadcasterId, string authCode, string subscriptionId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Retrieves a collection of event sub subscriptions including their cost details.
        /// </summary>
        /// <param name="broadcasterId">id of the broadcaster</param>
        /// <param name="authCode">authorization code</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The collection of event sub subscriptions</returns>
        Task<HelixSubscriptionResponse<Subscription>> GetEventSubSubscriptionAsync(string broadcasterId, string authCode, CancellationToken cancellationToken);

        /// <summary>
        /// Creates an event sub subscription.
        /// </summary>
        /// <param name="broadcasterId">id of the broadcaster</param>
        /// <param name="authCode">authorization code</param>
        /// <param name="sessionId">id of the event sub session</param>
        /// <param name="subscriptionType">subscription type</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The event sub subscription</returns>
        Task<HelixSubscriptionResponse<Subscription>> CreateEventSubSubscriptionAsync(string broadcasterId, string authCode, string sessionId, SubscriptionType subscriptionType, CancellationToken cancellationToken);
    }
}
