// ------------------------------------------------------------------------------------
// AuthHandlerMiddleware.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using netca.Infrastructure.Services;

namespace netca.Api.Middlewares
{
    /// <summary>
    /// AuthHandlerMiddleware
    /// </summary>
    public class AuthHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSetting _appSetting;
        private readonly ILogger<AuthHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="appSetting"></param>
        /// <param name="logger"></param>
        public AuthHandlerMiddleware(RequestDelegate next, AppSetting appSetting, ILogger<AuthHandlerMiddleware> logger)
        {
            _next = next;
            _appSetting = appSetting;
            _logger = logger;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var whitelistPathSegment = _appSetting.AuthorizationServer.WhiteListPathSegment.Split(",").ToList();
            var requiredCheck = !whitelistPathSegment.Any(item => context.Request.Path.StartsWithSegments(item));

            if (requiredCheck)
            {
                _logger.LogDebug("Authenticating");
                var auth = await CheckAuthAsync(context);
                if (!auth.Succeeded)
                {
                    await context.ChallengeAsync();
                    return;
                }
            }

            await _next(context);
        }

        private static async Task<AuthenticateResult> CheckAuthAsync(HttpContext context)
        {
            var auth = context.RequestServices.GetRequiredService<IAuthenticationService>();
            return await auth.AuthenticateAsync(context, scheme: JwtBearerDefaults.AuthenticationScheme);
        }
    }

    /// <summary>
    /// AuthHandlerMiddlewareExtensions
    /// </summary>
    public static class AuthHandlerMiddlewareExtensions
    {
        /// <summary>
        /// UseAuthHandler
        /// </summary>
        /// <param name="builder"></param>
        public static void UseAuthHandler(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseMiddleware<AuthHandlerMiddleware>();
        }

        /// <summary>
        /// AddPermissions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSetting"></param>
        public static void AddPermissions(this IServiceCollection services, AppSetting appSetting)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = appSetting.AuthorizationServer.Address;
                options.Audience = appSetting.AuthorizationServer.Service;
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                var policy = appSetting.AuthorizationServer.Policy;
                policy.ForEach(p =>
                {
                    if (p != null)
                        options.AddPolicy(p.Name, pol => pol.Requirements.Add(new Permission(p.Name)));
                });
            });
        }
    }
}