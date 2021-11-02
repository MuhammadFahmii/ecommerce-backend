// ------------------------------------------------------------------------------------
// AuthHandlerMiddleware.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Extensions;
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
            var serviceName = _appSetting.AuthorizationServer.Service;
            var whitelistPathSegment = _appSetting.AuthorizationServer.WhiteListPathSegment != null ?
                                       _appSetting.AuthorizationServer.WhiteListPathSegment.Split(",").ToList()
                                       : new List<string>();
            var requiredCheck = !whitelistPathSegment.Any(item => context.Request.Path.StartsWithSegments(item));

            if (requiredCheck)
            {
                _logger.LogDebug($"Authenticating");
                var auth = await CheckAuthAsync(context);
                if (!auth.Succeeded)
                {
                    await context.ChallengeAsync();
                    return;
                }

                var policyName = GeneratePolicy(context, serviceName);

                var policy = AddPolicyToContext(context, policyName);

                if (policy is { IsCheck: true })
                {
                    _logger.LogDebug($"Checking permission {policy.Name}");
                    var permission = await CheckPermissionAsync(context, policy.Name);
                    if (!permission.Succeeded)
                    {
                        await context.ForbidAsync();
                        return;
                    }
                }
            }

            await _next(context);
        }

        private Policy AddPolicyToContext(HttpContext context, string policyName)
        {
            _logger.LogDebug($"Getting info permission from config");
            var policy = GetPolicy(policyName);
            if (policy == null)
            {
                if (policyName.Contains("_"))
                {
                    policyName = policyName[..policyName.LastIndexOf("_", StringComparison.Ordinal)];
                }

                policy = GetPolicy(policyName);
            }

            _logger.LogDebug($"Add CurrentPolicyName {policyName} to http context");
            context.Items.Add("CurrentPolicyName", policyName);

            return policy;
        }

        private string GeneratePolicy(HttpContext context, string serviceName)
        {
            _logger.LogDebug($"GeneratePolicy");
            var path = context.Request.Path.ToString().Split("/").ToList();
            return StringExtensions.ToPolicyNameFormat(serviceName, context.Request.Method, path);
        }

        private static async Task<AuthenticateResult> CheckAuthAsync(HttpContext context)
        {
            var auth = context.RequestServices.GetRequiredService<IAuthenticationService>();
            return await auth.AuthenticateAsync(context, scheme: JwtBearerDefaults.AuthenticationScheme);
        }

        private static async Task<AuthorizationResult> CheckPermissionAsync(HttpContext context, string policy)
        {
            var permission = context.RequestServices.GetRequiredService<IAuthorizationService>();
            return await permission.AuthorizeAsync(context.User, null, policy);
        }

        private Policy GetPolicy(string policy)
        {
            _logger.LogDebug($"Get Policy {policy} if exists");
            var policyList = _appSetting.AuthorizationServer.Policy;
            return policyList.Count.Equals(0) ? null : policyList.FirstOrDefault(x => x.Name.ToLower().Equals(policy.ToLower()));
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
                var policy = appSetting.AuthorizationServer.Policy ?? new List<Policy>();
                policy.ForEach(p =>
                {
                    options.AddPolicy(p.Name, pol => pol.Requirements.Add(new Permission(p.Name)));
                });
            });
        }
    }
}