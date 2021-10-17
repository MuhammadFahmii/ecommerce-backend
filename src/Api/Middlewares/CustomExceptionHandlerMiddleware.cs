// ------------------------------------------------------------------------------------
// CustomExceptionHandlerMiddleware.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Net;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace netca.Api.Middlewares
{
     /// <summary>
    /// CustomExceptionHandlerMiddleware
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// CustomExceptionHandlerMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong: {ex.Source} {ex.Message}");
                await HandleExceptionAsync(httpContext, _logger, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ILogger logger, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(
                        JsonApiExtensions.ToJsonApi(new object(),
                        new Status{
                            Code = (int)code,
                            Desc = validationException.Errors
                        }
                    ), JsonExtensions.ErrorSerializerSettings());
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(
                        JsonApiExtensions.ToJsonApi(new object(),
                        new Status{
                            Code = (int)code,
                            Desc = badRequestException.Message
                        }
                    ), JsonExtensions.ErrorSerializerSettings());
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(
                        JsonApiExtensions.ToJsonApi(new object(),
                        new Status{
                            Code = (int)code,
                            Desc = exception.Message
                        }
                    ), JsonExtensions.ErrorSerializerSettings());
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (!string.IsNullOrEmpty(result)) return context.Response.WriteAsync(result);
            result = JsonConvert.SerializeObject(
                JsonApiExtensions.ToJsonApi(new object(),
                    new Status{
                        Code = (int)code,
                        Desc = "Internal Server Error"
                    }
                ), JsonExtensions.ErrorSerializerSettings());
            logger.LogError($"Internal Server Error: {exception.Source} {exception.Message}");
            return context.Response.WriteAsync(result);
        }
        
    }

    /// <summary>
    /// CustomExceptionHandlerMiddlewareExtensions
    /// </summary>
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// UseCustomExceptionHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static void UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
