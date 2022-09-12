// ------------------------------------------------------------------------------------
// ConfigureServices.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Buffers;
using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using JsonApiSerializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using netca.Api.Filters;
using netca.Api.Handlers;
using netca.Api.Middlewares;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;

namespace netca.Api;

/// <summary>
/// ConfigureServices
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// AddApiServices
    /// </summary>
    /// <param name="services"></param>
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    /// <returns></returns>
    public static void AddApiServices(this IServiceCollection services, IWebHostEnvironment? environment,
        AppSetting appSetting)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        services.AddScoped<ApiDevelopmentFilterAttribute>();
        services.AddScoped<ApiAuthenticationFilterAttribute>();
        if (environment?.EnvironmentName == "Test")
        {
            services.AddLocalPermissions(appSetting);
        }
        else
        {
            services.AddPermissions(appSetting);
        }
        
        services.AddMvcCore();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
        services
            .AddControllers(options =>
            {
                var serializerSettings = new JsonApiSerializerSettings();
                var jsonApiFormatter =
                    new NewtonsoftJsonOutputFormatter(serializerSettings, ArrayPool<char>.Shared, new MvcOptions(), new MvcNewtonsoftJsonOptions());
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
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            }); 
            services.AddFluentValidationAutoValidation(config => 
            {
                config.DisableDataAnnotationsValidation = true;
            }).AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<IApplicationDbContext>();

        services.AddCors();

        services.AddOptions();

        services.AddCompressionHandler();
        
        services.AddHealthCheck(appSetting);

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
            configure.Title = appSetting.App.Title;
            configure.Description = appSetting.App.Description;
            configure.Version = appSetting.App.Version;
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
                    Name = appSetting.App.AppContact.Company,
                    Email = appSetting.App.AppContact.Email,
                    Url = appSetting.App.AppContact.Uri
                };
            };
        });

        services.AddEndpointsApiExplorer();
    }

}