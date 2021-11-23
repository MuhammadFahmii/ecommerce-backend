using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using netca.Api.Handlers;
using netca.Application.Common.Models;
using Serilog;

namespace netca.Api
{
    /// <summary>
    /// Program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                Log.Information("Starting host");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var services = new ServiceCollection();
            var appSetting = new AppSetting();

            services.AddOptions();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, true);

                    config.AddEnvironmentVariables();

                    config.AddCommandLine(args);
                })
                .UseKestrel((hostingContext, option) =>
                {
                    services.Configure<AppSetting>(hostingContext.Configuration);
                    appSetting = services
                        .BuildServiceProvider()
                        .GetService<IOptionsSnapshot<AppSetting>>()?.Value ?? new AppSetting();

                    option.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(appSetting.Kestrel.KeepAliveTimeoutInM);
                    option.Limits.MinRequestBodyDataRate = new MinDataRate(
                        appSetting.Kestrel.MinRequestBodyDataRate.BytesPerSecond,
                        TimeSpan.FromSeconds(appSetting.Kestrel.MinRequestBodyDataRate.GracePeriod)
                    );
                    option.Limits.MinResponseDataRate = new MinDataRate(
                        appSetting.Kestrel.MinResponseDataRate.BytesPerSecond,
                        TimeSpan.FromSeconds(appSetting.Kestrel.MinResponseDataRate.GracePeriod)
                    );
                    option.AddServerHeader = false;
                })
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    services.Configure<AppSetting>(hostingContext.Configuration);
                    services.AddMemoryCache();
                    appSetting = services
                        .BuildServiceProvider()
                        .GetService<IOptionsSnapshot<AppSetting>>()?.Value ?? new AppSetting();
                    var memoryCache = services.BuildServiceProvider().GetRequiredService<IMemoryCache>();
                    var sink = new LogEventSinkHandler(appSetting, memoryCache);
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration).WriteTo
                        .Sink(sink);
                });
        }
    }
}