// ------------------------------------------------------------------------------------
// AddHealthCheckHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.Network;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using netca.Application.Common.Models;

namespace netca.Api.Handlers;

/// <summary>
/// AddHealthCheckHandler
/// </summary>
public static class AddHealthCheckHandler
{
    private const string HealthQuery = Constants.DefaultHealthCheckQuery;

    /// <summary>
    /// AddHealthCheck
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appSetting"></param>
    public static void AddHealthCheck(this IServiceCollection services, AppSetting appSetting)
    {
        var ums = new Uri(appSetting.AuthorizationServer.Address);
        var gateWay = new Uri(appSetting.AuthorizationServer.Gateway);

        void UmsSetup(TcpHealthCheckOptions x) => x.AddHost(ums.Host,
            Convert.ToInt32(appSetting.AuthorizationServer.Address.Split(":")[2]));

        void GateWaySetup(TcpHealthCheckOptions x) => x.AddHost(gateWay.Host, 443);
        services.AddHealthChecks().AddCheck<SystemMemoryHealthCheck>(Constants.DefaultHealthCheckMemoryUsage,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds));
        services.AddHealthChecks().AddCheck<SystemCpuHealthCheck>(Constants.DefaultHealthCheckCpuUsage,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds));
        services.AddHealthChecks().AddSqlServer(
            appSetting.ConnectionStrings.DefaultConnection,
            name: Constants.DefaultHealthCheckDatabaseName,
            healthQuery: HealthQuery,
            failureStatus: HealthStatus.Degraded,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds)
        );
        services.AddHealthChecks().AddTcpHealthCheck(
            UmsSetup,
            Constants.DefaultHealthCheckUmsName,
            HealthStatus.Degraded,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds)
        );
        services.AddHealthChecks().AddTcpHealthCheck(
            GateWaySetup,
            Constants.DefaultHealthCheckGateWayName,
            HealthStatus.Degraded,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds)
        );
        services.AddHealthChecks().AddRedis(
            appSetting.RedisServer.Server,
            Constants.DefaultHealthCheckRedisName,
            HealthStatus.Degraded,
            timeout: TimeSpan.FromSeconds(Constants.DefaultHealthCheckTimeoutInSeconds)
        );
    }

    /// <summary>
    /// UseHealthCheck
    /// </summary>
    /// <param name="app"></param>
    public static void UseHealthCheck(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UiResponseWriter.WriteHealthCheckUiResponse
        });
    }
}

/// <summary>
/// SystemCpuHealthCheck
/// </summary>
public class SystemCpuHealthCheck : IHealthCheck
{
    /// <summary>
    /// CheckHealthAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

        await Task.Delay(500, cancellationToken);

        var endTime = DateTime.UtcNow;
        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

        var status = HealthStatus.Healthy;

        var data = new Dictionary<string, object> { { "Cpu_Usage", cpuUsageTotal } };

        if (cpuUsageTotal > Constants.DefaultHealthCheckPercentageUsedDegraded)
            status = HealthStatus.Degraded;

        var result = new HealthCheckResult(status, null, null, data);

        return await Task.FromResult(result);
    }
}

/// <summary>
/// SystemMemoryHealthCheck
/// </summary>
public class SystemMemoryHealthCheck : IHealthCheck
{
    /// <summary>
    /// CheckHealthAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var client = new MemoryMetricsClient();
        var metrics = client.GetMetrics();
        var percentUsed = 100 * metrics.Used / metrics.Total;

        var status = HealthStatus.Healthy;
        if (percentUsed > Constants.DefaultHealthCheckPercentageUsedDegraded)
            status = HealthStatus.Degraded;

        var data = new Dictionary<string, object>
        {
            { "Total_MB", metrics.Total },
            { "Used_MB", metrics.Used },
            { "Free_MB", metrics.Free }
        };

        var result = new HealthCheckResult(status, null, null, data);

        return await Task.FromResult(result);
    }
}

/// <summary>
/// MemoryMetrics
/// </summary>
public class MemoryMetrics
{
    /// <summary>
    /// Gets or sets total memory
    /// </summary>
    public double Total { get; set; }

    /// <summary>
    /// Gets or sets used memory
    /// </summary>
    public double Used { get; set; }

    /// <summary>
    /// Gets or sets free memory
    /// </summary>
    public double Free { get; set; }
}

/// <summary>
/// MemoryMetricsClient
/// </summary>
public class MemoryMetricsClient
{
    /// <summary>
    /// GetMetrics
    /// </summary>
    /// <returns></returns>
    public MemoryMetrics GetMetrics()
    {
        var metrics = IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

        return metrics;
    }

    private bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                             RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    private static MemoryMetrics GetWindowsMetrics()
    {
        var info = new ProcessStartInfo
        {
            FileName = "wmic",
            Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
            RedirectStandardOutput = true
        };

        using var process = Process.Start(info);
        var output = process!.StandardOutput.ReadToEnd();

        var lines = output.Trim().Split("\n");
        var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
        var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics
        {
            Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0),
            Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0)
        };
        metrics.Used = metrics.Total - metrics.Free;

        return metrics;
    }

    private static MemoryMetrics GetUnixMetrics()
    {
        var info = new ProcessStartInfo("free -m")
        {
            FileName = "/bin/bash",
            Arguments = "-c \"free -m\"",
            RedirectStandardOutput = true
        };

        using var process = Process.Start(info);
        var output = process!.StandardOutput.ReadToEnd();

        var lines = output.Split("\n");
        var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics
        {
            Total = double.Parse(memory[1]),
            Used = double.Parse(memory[2]),
            Free = double.Parse(memory[3])
        };

        return metrics;
    }
}

/// <summary>
/// UiHealthReport
/// </summary>
public class UiHealthReport
{
    /// <summary>
    /// Gets or sets status
    /// </summary>
    public UiHealthStatus Status { get; set; }

    /// <summary>
    /// Gets or sets totalDuration
    /// </summary>
    public TimeSpan TotalDuration { get; set; }

    /// <summary>
    /// Gets entries
    /// </summary>
    public Dictionary<string, UiHealthReportEntry> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UiHealthReport"/> class.
    /// </summary>
    /// <param name="entries"></param>
    /// <param name="totalDuration"></param>
    public UiHealthReport(Dictionary<string, UiHealthReportEntry> entries, TimeSpan totalDuration)
    {
        Entries = entries;
        TotalDuration = totalDuration;
    }

    /// <summary>
    /// CreateFrom
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    public static UiHealthReport CreateFrom(HealthReport report)
    {
        var uiReport = new UiHealthReport(new Dictionary<string, UiHealthReportEntry>(), report.TotalDuration)
        {
            Status = (UiHealthStatus)report.Status,
        };

        foreach (var (key, value) in report.Entries)
        {
            var entry = new UiHealthReportEntry
            {
                Data = value.Data,
                Description = value.Description,
                Duration = value.Duration,
                Status = (UiHealthStatus)value.Status
            };

            if (value.Exception != null)
            {
                var message = value.Exception?
                    .Message;

                entry.Exception = message;
                entry.Description = value.Description ?? message;
            }

            uiReport.Entries.Add(key, entry);
        }

        return uiReport;
    }
}

/// <summary>
/// UiHealthStatus
/// </summary>
public enum UiHealthStatus
{
    /// <summary>
    /// Unhealthy
    /// </summary>
    Unhealthy = 0,

    /// <summary>
    /// Degraded
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// Healthy
    /// </summary>
    Healthy = 2
}

/// <summary>
/// UiHealthReportEntry
/// </summary>
public class UiHealthReportEntry
{
    /// <summary>
    /// Gets or Sets Data
    /// </summary>
    /// <value></value>
    public IReadOnlyDictionary<string, object>? Data { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    /// <value></value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or Sets Duration
    /// </summary>
    /// <value></value>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or Sets Exception
    /// </summary>
    /// <value></value>
    public string? Exception { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    /// <value></value>
    public UiHealthStatus Status { get; set; }
}

/// <summary>
/// UiResponseWriter
/// </summary>
public static class UiResponseWriter
{
    private static readonly Lazy<JsonSerializerOptions> Options = new(CreateJsonOptions);

    /// <summary>
    /// WriteHealthCheckUIResponse
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="report"></param>
    /// <returns></returns>
    public static async Task WriteHealthCheckUiResponse(HttpContext httpContext, HealthReport report)
    {
        httpContext.Response.ContentType = Constants.HeaderJson;

        var uiReport = UiHealthReport
            .CreateFrom(report);

        await using var responseStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(responseStream, uiReport, Options.Value);
        await httpContext.Response.BodyWriter.WriteAsync(responseStream.ToArray());
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var jsonSerializeOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        jsonSerializeOptions.Converters.Add(new JsonStringEnumConverter());

        jsonSerializeOptions.Converters.Add(new TimeSpanConverter());

        return jsonSerializeOptions;
    }
}

/// <summary>
/// TimeSpanConverter
/// </summary>
internal class TimeSpanConverter : JsonConverter<TimeSpan>
{
    /// <summary>
    /// Read
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default;
    }

    /// <summary>
    /// Write
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}