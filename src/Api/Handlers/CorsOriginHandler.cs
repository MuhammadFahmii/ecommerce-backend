// ------------------------------------------------------------------------------------
// CorsOriginHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using netca.Application.Common.Models;

namespace netca.Api.Handlers
{
    /// <summary>
    /// CorsOriginHandler
    /// </summary>
    public static class CorsOriginHandler
    {
        /// <summary>
        /// ApplyCorsOrigin
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="appSetting"></param>
        public static void ApplyCorsOrigin(IApplicationBuilder builder, AppSetting appSetting)
        {
            var origin = appSetting.CorsOrigin;
            builder.UseCors(
                options => options.WithOrigins(origin)
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod().AllowAnyHeader()
            );
        }
    }

    /// <summary>
    /// UseCorsOriginHanlderextension
    /// </summary>
    public static class UseCorsOriginHanldereextension
    {
        /// <summary>
        /// UseCorsOriginHanlder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static void UseCorsOriginHanlder(this IApplicationBuilder builder, AppSetting appSetting)
        {
            CorsOriginHandler.ApplyCorsOrigin(builder, appSetting);
        }
    }
}
