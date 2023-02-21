using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharpTwitch.Core.Settings;

namespace SharpTwitch.Core
{
    public static class Configuration
    {
        public static IServiceCollection AddTwitchCore(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<CoreSettings>(configurationManager.GetSection(CoreSettings.Key));
            services.AddSingleton<ICoreSettings>(options => options.GetRequiredService<IOptions<CoreSettings>>().Value);
            services.AddSingleton<IApiCore, ApiCore>();
            return services;
        }
    }
}
