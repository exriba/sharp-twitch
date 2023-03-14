using Microsoft.Extensions.DependencyInjection;
using SharpTwitch.Helix.Rewards;
using SharpTwitch.Helix.Subscriptions;
using SharpTwitch.Helix.Users;

namespace SharpTwitch.Helix
{
    /// <summary>
    /// SharpTwitch.Helix Configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Registers Helix interfaces into the DI container. <see cref="SharpTwitch.Helix.HelixApi"/>
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTwitchHelix(this IServiceCollection services)
        {
            services.AddTransient<IUsers, Users.Users>();
            services.AddTransient<ICustomRewards, Rewards.CustomRewards>();
            services.AddTransient<ISubscriptions, Subscriptions.Subscriptions>();
            services.AddTransient<HelixApi>();
            return services;
        }
    }
}
