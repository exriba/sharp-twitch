using Microsoft.Extensions.DependencyInjection;

namespace SharpTwitch
{
    /// <summary>
    /// SharpTwitch Configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Registers TwitchApi into the DI container. <see cref="SharpTwitch.TwitchAPI"/>
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTwitchApi(this IServiceCollection services)
        {
            services.AddTransient<TwitchAPI>();
            return services;
        }
    }
}
