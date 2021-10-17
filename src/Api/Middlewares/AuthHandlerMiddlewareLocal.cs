// ------------------------------------------------------------------------------------
// AuthHandlerMiddlewareLocal.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using netca.Api.Handlers;
using netca.Application.Common.Models;

namespace netca.Api.Middlewares
{
    /// <summary>
    /// AuthHandlerMiddlewareLocal
    /// </summary>
    public class AuthHandlerMiddlewareLocal
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// AuthHandlerMiddlewareLocal
        /// </summary>
        /// <param name="next"></param>
        public AuthHandlerMiddlewareLocal(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }

    /// <summary>
    /// AuthHandlerMiddlewareLocalExtensions
    /// </summary>
    public static class AuthHandlerMiddlewareLocalExtensions
    {
        /// <summary>
        /// UseAuthHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static void UseLocalAuthHandler(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseMiddleware<AuthHandlerMiddlewareLocal>();
        }

        /// <summary>
        /// AddPermissions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static void AddLocalPermissions(this IServiceCollection services, AppSetting appSetting)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = LocalAuthenticationHandler.AuthScheme;
                options.DefaultAuthenticateScheme = LocalAuthenticationHandler.AuthScheme;
                options.DefaultChallengeScheme = LocalAuthenticationHandler.AuthScheme;
            }).AddScheme<AuthenticationSchemeOptions, LocalAuthenticationHandler>(
                LocalAuthenticationHandler.AuthScheme, _ => { });
        }
    }
}