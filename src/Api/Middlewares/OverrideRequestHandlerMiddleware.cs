// ------------------------------------------------------------------------------------
// OverrideRequestHandlerMiddleware.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;

namespace netca.Api.Middlewares;

/// <summary>
/// OverrideRequestHandlerMiddleware
/// </summary>
public class OverrideRequestHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IRedisService _redisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OverrideRequestHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="redisService"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public OverrideRequestHandlerMiddleware(RequestDelegate next, IRedisService redisService,
        ILogger<OverrideRequestHandlerMiddleware> logger)
    {
        _next = next;
        _redisService = redisService;
        _logger = logger;
    }

    /// <summary>
    /// InvokeAsync
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogDebug("Overriding Request");

        if (context.Request.Path.StartsWithSegments("/api"))
        {
            var requestIfNoneMatch = context.Request.Headers[Constants.HeaderIfNoneMatch].ToString() ?? "";

            if (!string.IsNullOrEmpty(requestIfNoneMatch))
            {
                var encodedEntity = await _redisService.GetAsync(requestIfNoneMatch);
                if (!string.IsNullOrEmpty(encodedEntity))
                {
                    const int code = (int)HttpStatusCode.NotModified;
                    context.Response.StatusCode = code;
                    return;
                }
            }
        }

        await _next(context);
    }
}

/// <summary>
/// OverrideRequestMiddlewareExtensions
/// </summary>
public static class OverrideRequestMiddlewareExtensions
{
    /// <summary>
    /// UseOverrideRequestHandler
    /// </summary>
    /// <param name="builder"></param>
    public static void UseOverrideRequestHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<OverrideRequestHandlerMiddleware>();
    }
}