using System.Buffers;
using System.Linq;
using System.Text;
using FluentValidation.AspNetCore;
using JsonApiSerializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netca.Api.Filters;
using netca.Api.Handlers;
using netca.Api.Middlewares;
using netca.Api.Processors;
using netca.Application;
using netca.Application.Common.Extensions;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;

namespace netca.Api;

/// <summary>
/// Startup
/// </summary>
public class Startup
{
    private ILogger<Startup>? _logger;
    private IServiceCollection _services = null!;
    
    /// <summary>
    /// startup app
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration? configuration)
    {
        AppSetting = configuration.Get<AppSetting>();
    }

    /// <summary>
    /// Gets environment
    /// </summary>
    /// <value></value>
    private IWebHostEnvironment? Environment { get; set; }

    /// <summary>
    /// Gets AppSetting
    /// </summary>
    /// <value></value>
    private AppSetting AppSetting { get; }

    /// <summary>
    /// ConfigureServices
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        
        Environment = CreateServiceProvider(services).GetService<IWebHostEnvironment>();
        var loggerFactory = CreateServiceProvider(services).GetService<ILoggerFactory>();
        AppLoggingExtensions.LoggerFactory = loggerFactory;
        _logger = AppLoggingExtensions.CreateLogger<Startup>();
        
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

        services.AddSingleton(AppSetting);

        services.AddMemoryCache();

        services.AddHttpContextAccessor();

        services.AddApplication();

        services.AddInfrastructure(Environment, AppSetting);

        if (AppSetting.IsEnableAuth)
        {
            services.AddScoped<ApiAuthorizeFilterAttribute>();
        }
        services.AddScoped<ApiDevelopmentFilterAttribute>();

        if (Environment?.EnvironmentName == "Test")
        {
            services.AddLocalPermissions(AppSetting);
        }
        else
        {
            services.AddPermissions(AppSetting);
        }

        services.AddMvcCore(options =>
        {
            var serializerSettings = new JsonApiSerializerSettings();
#pragma warning disable 0618
            var jsonApiFormatter =
                new NewtonsoftJsonOutputFormatter(serializerSettings, ArrayPool<char>.Shared, new MvcOptions());
#pragma warning restore 0618

            options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
            options.OutputFormatters.Insert(0, jsonApiFormatter);

            options.EnableEndpointRouting = false;

            options.Filters.Add<ApiExceptionFilterAttribute>();
            options.Filters.Add(
                new ProducesResponseTypeAttribute(typeof(object), (int)System.Net.HttpStatusCode.Unauthorized));
            options.Filters.Add(
                new ProducesResponseTypeAttribute(typeof(object), (int)System.Net.HttpStatusCode.Forbidden));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(object),
                (int)System.Net.HttpStatusCode.InternalServerError));
        });

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            })
            .AddFluentValidation(fv =>
            {
                fv.AutomaticValidationEnabled = false;
                fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>();
            });

        services.AddCors();

        services.AddOptions();

        services.AddCompressionHandler();

        services.AddHealthCheck(AppSetting);

        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddOpenApiDocument(configure =>
        {
            if (Environment?.IsProduction() ?? false)
            {
                configure.OperationProcessors.Insert(0, new MyControllerProcessor());
            }

            configure.Title = AppSetting.App.Title;
            configure.Description = AppSetting.App.Description;
            configure.Version = AppSetting.App.Version;
            configure.AllowNullableBodyParameters = false;
            configure.AllowReferencesWithProperties = true;
            configure.IgnoreObsoleteProperties = true;
            configure.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the text box: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            configure.PostProcess = document =>
            {
                document.Info.Contact = new OpenApiContact
                {
                    Name = AppSetting.App.AppContact.Company,
                    Email = AppSetting.App.AppContact.Email,
                    Url = AppSetting.App.AppContact.Uri
                };
            };
        });

        _services = services;
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMigrationsHandler(AppSetting);
        app.UseCorsOriginHandler(AppSetting);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            RegisteredServicesPage(app);
            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);
        }
        else if (env.IsProduction())
        {
            app.UseHsts();
        }

        app.UseResponseCompression();

        if (!AppSetting.IsEnableDetailError)
        {
            _logger?.LogDebug("Activate exception middleware");
            app.UseCustomExceptionHandler();
        }
        else
        {
            _logger?.LogWarning("enable detail error response");
        }

        app.UseRouting();

        if (AppSetting.IsEnableAuth)
        {
            _logger?.LogInformation("Activate auth middleware");

            if (env.EnvironmentName == "Test")
            {
                app.UseLocalAuthHandler();
            }
            else
            {
                app.UseAuthHandler();
            }
        }
        else
        {
            _logger?.LogWarning("Disable Auth middleware");
        }

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseOverrideRequestHandler();

        app.UseOverrideResponseHandler();

        app.UseHealthCheck();

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
    }
    
    private static ServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }

    private void RegisteredServicesPage(IApplicationBuilder app)
    {
        app.Map("/services", builder => builder.Run(async context =>
        {
            var sb = new StringBuilder();
            sb.Append("<h1>Registered Services</h1>");
            sb.Append("<table><thead>");
            sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
            sb.Append("</thead><tbody>");
            foreach (var svc in _services)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                sb.Append($"<td>{svc.Lifetime}</td>");
                sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
            await context.Response.WriteAsync(sb.ToString());
        }));
    }
}