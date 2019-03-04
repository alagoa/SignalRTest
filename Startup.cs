using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignalRTest.Hubs;

namespace SignalRTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new ConsoleLoggerProvider(LogLevel.Trace));

            Globals.LoggerFactory = loggerFactory;
            Globals.SystemLogger = loggerFactory.CreateLogger("sys");

            Globals.SystemLogger.LogTrace("-=-=-=- trace -=-=-=-");
            Globals.SystemLogger.LogDebug("-=-=-=- debug -=-=-=-");
            Globals.SystemLogger.LogInformation("-=-=-=- info -=-=-=-");
            Globals.SystemLogger.LogWarning("-=-=-=- warn -=-=-=-");
            Globals.SystemLogger.LogError("-=-=-=- error -=-=-=-");
            Globals.SystemLogger.LogCritical("-=-=-=- critic -=-=-=-");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<MyHub>("/general");
            });
        }
    }
}
