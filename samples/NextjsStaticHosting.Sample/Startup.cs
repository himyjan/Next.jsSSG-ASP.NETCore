using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextjsStaticHosting;

namespace NextjsStaticHostingSample
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Not necessary for the sample, added to demonstrate that controllers still work as usual
            services.AddControllers();

            // Step 1: Add Next.js hosting support and configure the root path (this is relative to IHostEnvironment.ContentRootPath).
            services.Configure<NextjsStaticHostingOptions>(this.configuration.GetSection("NextjsStaticHosting"));
            services.AddNextjsStaticHosting(options => options.RootPath = "wwwroot/ClientApp");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Not necessary for the sample, added to demonstrate that controllers still work as usual
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                // Step 2: Register dynamic endpoints to serve the correct HTML files at the right request paths.
                // Endpoints are created dynamically based on HTML files found under the specified RootPath during startup.
                // Endpoints are currently NOT refreshed if the files later change on disk.
                endpoints.MapNextjsStaticHtmls();
            });

            // Step 3: Serve other files the app may require (e.g. js, css files within the RootPath).
            app.UseNextjsStaticHosting();
        }
    }
}
