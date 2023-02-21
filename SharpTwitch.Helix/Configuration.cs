using Microsoft.Extensions.DependencyInjection;
using SharpTwitch.Helix.Rewards;
using SharpTwitch.Helix.Subscriptions;

namespace SharpTwitch.Helix
{
    public static class Configuration
    {
        public static IServiceCollection AddTwitchHelix(this IServiceCollection services)
        {
            services.AddTransient<ICustomRewards, CustomRewards>();
            services.AddTransient<ISubscriptions, Subscriptions.Subscriptions>();
            services.AddTransient<HelixApi>();
            return services;
        }
    }
}
