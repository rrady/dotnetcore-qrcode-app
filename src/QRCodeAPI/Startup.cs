using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using QRCodeAPI.CacheStore;
using QRCodeAPI.Client;
using QRCodeAPI.Client.Types;
using QRCodeAPI.Middlewares;
using QRCodeAPI.Services;

namespace QRCodeAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient<IQrClient, GoQrClient>();
            services.AddSingleton(typeof(IFileProvider), service =>
                new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            services.AddTransient<IQrService, GoQrService>();
            services.AddMemoryCache();
            services.AddTransient<ICacheStore, InMemoryCacheStore>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}