using mailBlazzorApp.Library.Configuration;
using mailBlazzorApp.Library.Services;
using Microsoft.Extensions.DependencyInjection;

namespace mailBlazzorApp.Library
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMailLibrary(this IServiceCollection services)
        {
            services.AddScoped<IMailService,MailService>();
            return services;
        }
        
        public static IServiceCollection AddMailConfig(this IServiceCollection services, MailSettings mailSettings)
        {
            services.AddSingleton(mailSettings);
            return services;
        }
    }
}