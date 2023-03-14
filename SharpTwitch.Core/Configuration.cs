using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharpTwitch.Core.Settings;

namespace SharpTwitch.Core
{
    /// <summary>
    /// SharpTwitch.Core Configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Registers ApiCore into the DI container. <see cref="SharpTwitch.Core.IApiCore"/>
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTwitchCore(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.Configure<CoreSettings>(configurationManager.GetSection(CoreSettings.Key));
            services.AddSingleton<ICoreSettings>(options => options.GetRequiredService<IOptions<CoreSettings>>().Value);
            services.AddSingleton<IApiCore, ApiCore>();
            return services;
        }
    }
}
