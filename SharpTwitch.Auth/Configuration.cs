using Microsoft.Extensions.DependencyInjection;

namespace SharpTwitch.Auth
{
    /// <summary>
    /// SharpTwitch.Auth Configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Registers Twitch AuthApi into the DI container. <see cref="SharpTwitch.Auth.IAuthApi"/>
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTwitchAuth(this IServiceCollection services)
        {
            services.AddTransient<IAuthApi, AuthApi>();
            return services;
        }
    }
}
