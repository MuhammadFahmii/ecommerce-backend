// ------------------------------------------------------------------------------------
// OverrideResponseHandlerMiddleware.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using Newtonsoft.Json.Linq;

namespace netca.Api.Middlewares;

/// <summary>
/// OverrideResponseHandlerMiddleware
/// </summary>
public class OverrideResponseHandlerMiddleware
{
    private readonly IRedisService _redisService;
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="OverrideResponseHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next"></param>
    /// <param name="redisService"></param>
    /// <param name="appSetting"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public OverrideResponseHandlerMiddleware(RequestDelegate next, IRedisService redisService, AppSetting appSetting,
        ILogger<OverrideResponseHandlerMiddleware> logger)
    {
        _next = next;
        _appSetting = appSetting;
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
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            _logger.LogDebug($"Overriding Response");
            do
            {
                var watch = new Stopwatch();
                watch.Start();
                var originalBody = context.Response.Body;
                try
                {
                    int statusCode;
                    string responseBody;
                    string policyName;
                    await using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;
                        await _next(context);
                        policyName = (string)context.Items["CurrentPolicyName"]!;
                        statusCode = context.Response.StatusCode;
                        memStream.Position = 0;

                        if (statusCode != 200)
                        {
                            await memStream.CopyToAsync(originalBody);
                            break;
                        }

                        if (!context.Response.ContentType.Contains("application/json"))
                        {
                            await memStream.CopyToAsync(originalBody);
                            break;
                        }

                        responseBody = await new StreamReader(memStream).ReadToEndAsync();
                    }

                    watch.Stop();
                    var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                    context = await RedisCachingAsync(policyName, context, responseBody);
                    var buffer =
                        Encoding.UTF8.GetBytes(ToJsonApi(statusCode, responseTimeForCompleteRequest, responseBody));

                    context.Response.ContentLength = buffer.Length;
                    await using var output = new MemoryStream(buffer);
                    output.Position = 0;
                    await output.CopyToAsync(originalBody);

                    context.Response.Body = originalBody;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Error Overriding Response {Ex}", ex.Message);
                    throw;
                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            } while (false);
        }
        else
        {
            await _next(context);
        }
    }

    private string ToJsonApi(int statusCode, long responseTimeForCompleteRequest, string responseBody)
    {
        var json = JObject.Parse(responseBody);
        json["responseTime"] = responseTimeForCompleteRequest;
        if (json["status"] is not JObject status)
        {
            var stat = new JObject
            {
                { "code", statusCode },
                { "desc", ReasonPhrases.GetReasonPhrase(statusCode) }
            };
            json.Add("status", stat);
        }
        else
        {
            status["code"] = statusCode;
            status["desc"] = ReasonPhrases.GetReasonPhrase(statusCode);
        }

        return json.ToString();
    }

    private async Task<HttpContext> RedisCachingAsync(string policyName, HttpContext context, string responseBody)
    {
        var requestIfNoneMatch = context.Request.Headers[Constants.HeaderIfNoneMatch].ToString() ?? "";
        if (string.IsNullOrEmpty(requestIfNoneMatch))
            return context;

        var policy = IsCache(policyName);
        if (policy is not { IsCache: true })
            return context;

        var key = await _redisService.SaveAsync(policy.Name, Constants.RedisSubKeyHttpRequest, responseBody);
        context.Response.Headers[Constants.HeaderETag] = key;
        return context;
    }

    private Policy IsCache(string policy)
    {
        var policyList = _appSetting.RedisServer.Policy;
        return (policyList.Count.Equals(0)
            ? null
            : policyList.SingleOrDefault(x => x.Name.ToLower().Equals(policy.ToLower())))!;
    }
}

/// <summary>
/// OverrideResponseMiddlewareExtensions
/// </summary>
public static class OverrideResponseMiddlewareExtensions
{
    /// <summary>
    /// UseOverrideResponseHandler
    /// </summary>
    /// <param name="builder"></param>
    public static void UseOverrideResponseHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<OverrideResponseHandlerMiddleware>();
    }
}