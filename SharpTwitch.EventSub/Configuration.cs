using Microsoft.Extensions.DependencyInjection;
using SharpTwitch.EventSub.Core.Handler;

namespace SharpTwitch.EventSub
{
    /// <summary>
    /// SharpTwitch.EventSub Configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Registers EventSub into the DI container. <see cref="SharpTwitch.EventSub.EventSub"/>
        /// </summary>
        /// <param name="services">service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddTwitchEventSub(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddSingleton<EventSub>();
            services.AddNotificationHandlers(typeof(INotificationHandler));
            return services;
        }

        private static IServiceCollection AddNotificationHandlers(this IServiceCollection services, params Type[] markers)
        {
            foreach (var marker in markers)
            {
                var types = marker.Assembly
                    .DefinedTypes.Where(x => typeof(INotificationHandler).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

                foreach (var type in types)
                    services.AddSingleton(typeof(INotificationHandler), type);
            }

            return services;
        }
    }
}
