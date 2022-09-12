// using System;
// using Microsoft.AspNetCore;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Server.Kestrel.Core;
// using Microsoft.Extensions.Caching.Memory;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Options;
// using netca.Api.Handlers;
// using netca.Application.Common.Models;
// using Serilog;
//
// namespace netca.Api;
//
// /// <summary>
// /// Program
// /// </summary>
// public static class Program
// {
//     /// <summary>
//     /// Main function
//     /// </summary>
//     /// <param name="args"></param>
//     public static void Main(string[] args)
//     {
//         var host = CreateWebHostBuilder(args).Build();
//         try
//         {
//             Log.Information("Starting host");
//             host.Run();
//         }
//         catch (Exception ex)
//         {
//             Log.Fatal(ex, "Host terminated unexpectedly");
//         }
//         finally
//         {
//             Log.CloseAndFlush();
//         }
//     }
//
//     private static IWebHostBuilder CreateWebHostBuilder(string[] args)
//     {
//         var services = new ServiceCollection();
//         var appSetting = new AppSetting();
//
//         services.AddOptions();
// #pragma warning disable 0618
//
//         return WebHost.CreateDefaultBuilder(args)
//             .ConfigureAppConfiguration((hostingContext, config) =>
//             {
//                 var env = hostingContext.HostingEnvironment;
//
//                 config.AddJsonFile("appsettings.json", optional: true, true)
//                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, true)
//                     .AddJsonFile($"appsettings.Local.json", optional: true, true);
//
//                 config.AddEnvironmentVariables();
//
//                 config.AddCommandLine(args);
//             })
//             .UseKestrel((hostingContext, option) =>
//             {
//                 services.Configure<AppSetting>(hostingContext.Configuration);
//                 appSetting = services
//                     .BuildServiceProvider()
//                     .GetService<IOptionsSnapshot<AppSetting>>()?.Value ?? new AppSetting();
//
//                 option.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(appSetting.Kestrel.KeepAliveTimeoutInM);
//                 option.Limits.MinRequestBodyDataRate = new MinDataRate(
//                     appSetting.Kestrel.MinRequestBodyDataRate.BytesPerSecond,
//                     TimeSpan.FromSeconds(appSetting.Kestrel.MinRequestBodyDataRate.GracePeriod)
//                 );
//                 option.Limits.MinResponseDataRate = new MinDataRate(
//                     appSetting.Kestrel.MinResponseDataRate.BytesPerSecond,
//                     TimeSpan.FromSeconds(appSetting.Kestrel.MinResponseDataRate.GracePeriod)
//                 );
//                 option.AddServerHeader = false;
//             })
//             .UseStartup<Startup>()
//             .UseSerilog((hostingContext, loggerConfiguration) =>
//             {
//                 services.Configure<AppSetting>(hostingContext.Configuration);
//                 services.AddMemoryCache();
//                 appSetting = services
//                     .BuildServiceProvider()
//                     .GetService<IOptionsSnapshot<AppSetting>>()?.Value ?? new AppSetting();
//                 var memoryCache = services.BuildServiceProvider().GetRequiredService<IMemoryCache>();
//                 var sink = new LogEventSinkHandler(appSetting, memoryCache);
//                 loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration).WriteTo
//                     .Sink(sink);
//             });
// #pragma warning restore 0618
//     }
// }

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;
using netca.Api;
using netca.Api.Handlers;
using netca.Api.Middlewares;
using netca.Application;
using netca.Application.Common.Extensions;
using netca.Application.Common.Models;
using netca.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration).WriteTo
        .Console();
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, false);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, false);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, false);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddCommandLine(args);
var appSetting = builder.Configuration.Get<AppSetting>();
var services2 = new ServiceCollection();
var serviceProvider = services2.BuildServiceProvider();
var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
AppLoggingExtensions.LoggerFactory = loggerFactory;
builder.WebHost.UseKestrel(option=>
{
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
});
builder.Services.AddSingleton(appSetting);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Environment, appSetting);
builder.Services.AddApiServices(builder.Environment, appSetting);
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
var app = builder.Build();
app.UseCorsOriginHandler(appSetting);
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsHandler(app.Environment, appSetting);
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    var option = new RewriteOptions();
    option.AddRedirect("^$", "swagger");
    app.UseRewriter(option);
}
else
{
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();
if (!appSetting.IsEnableDetailError)
{
    Log.Debug("Activate exception middleware");
    app.UseCustomExceptionHandler();
}
else
{
    Log.Warning("enable detail error response");
}
app.UseOverrideResponseHandler();

app.UseOpenApi(x =>
    x.PostProcess = (document, _) => { document.Schemes = new[] { OpenApiSchema.Https, OpenApiSchema.Http }; }
);

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/swagger";
    settings.EnableTryItOut = true;
});
app.UseMvc();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
Log.Information("Starting host");
app.Run();
