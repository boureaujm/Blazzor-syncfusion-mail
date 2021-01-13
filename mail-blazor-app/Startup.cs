using System;
using System.Reflection;
using BlazorApp.Configuration;
using mailBlazzorApp.Library;
using mailBlazzorApp.Library.Configuration;
using MediatR; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;


namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            var handlersAssembly = AppDomain.CurrentDomain.Load("mailBlazzorApp.Library");
            services.AddMediatR(handlersAssembly);
            
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMailLibrary();

            services.AddSyncfusionBlazor();
            
            ReadConfiguration(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

        }

        /// <summary>
        /// Read configuration from settings
        /// </summary>
        /// <param FolderName="services"></param>
        private void ReadConfiguration(IServiceCollection services)
        {
            var section = _configuration.GetSection(nameof(MailSettings));
            var mailSettings = section.Get<MailSettings>();
            services.AddMailConfig(mailSettings);
            
            var uiSection = _configuration.GetSection(nameof(InterfaceSettings));
            var uiSettings = uiSection.Get<InterfaceSettings>();
            services.AddInterfaceConfig(uiSettings);

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(uiSettings.SyncFusionKey);
        }
    }
}
