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
using ecommerce.Api;
using ecommerce.Api.Handlers;
using ecommerce.Api.Middlewares;
using ecommerce.Application;
using ecommerce.Application.Common.Extensions;
using ecommerce.Application.Common.Models;
using ecommerce.Infrastructure;
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

app.UseHealthCheck();
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

/// <summary>
/// Program
/// </summary>
public partial class Program { }
