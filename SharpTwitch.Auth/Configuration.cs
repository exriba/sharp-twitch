using Microsoft.Extensions.DependencyInjection;

namespace SharpTwitch.Auth
{
    public static class Configuration
    {
        public static IServiceCollection AddTwitchAuth(this IServiceCollection services)
        {
            services.AddTransient<IAuthApi, AuthApi>();
            return services;
        }
    }
}
