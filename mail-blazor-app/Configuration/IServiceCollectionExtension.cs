using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInterfaceConfig(this IServiceCollection services, InterfaceSettings uiSettings)
        {
            services.AddSingleton(uiSettings);
            return services;
        }
    }
}