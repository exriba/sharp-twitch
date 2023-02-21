using Microsoft.Extensions.DependencyInjection;

namespace SharpTwitch
{
    public static class Configuration
    {
        public static IServiceCollection AddTwitchApi(this IServiceCollection services)
        {
            services.AddTransient<TwitchAPI>();
            return services;
        }
    }
}
