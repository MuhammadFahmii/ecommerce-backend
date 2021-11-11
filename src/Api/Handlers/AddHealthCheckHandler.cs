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

namespace netca.Api.Handlers
{ /// <summary>
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
            void UmsSetup(TcpHealthCheckOptions x) => x.AddHost(ums.Host, Convert.ToInt32(appSetting.AuthorizationServer.Address.Split(":")[2]));
            void GateWaySetup(TcpHealthCheckOptions x) => x.AddHost(gateWay.Host, 443);
            services.AddHealthChecks().AddCheck<SystemMemoryHealthcheck>(Constants.DefaultHealthCheckMemoryUsage);
            services.AddHealthChecks().AddCheck<SystemCpuHealthcheck>(Constants.DefaultHealthCheckCpuUsage);
            services.AddHealthChecks().AddSqlServer(
                connectionString: appSetting.ConnectionStrings.DefaultConnection,
                name: Constants.DefaultHealthCheckDatabaseName,
                healthQuery: HealthQuery,
                failureStatus: HealthStatus.Degraded);
            services.AddHealthChecks().AddTcpHealthCheck(setup: UmsSetup, name: Constants.DefaultHealthCheckUmsName, failureStatus: HealthStatus.Degraded);
            services.AddHealthChecks().AddTcpHealthCheck(setup: GateWaySetup, name: Constants.DefaultHealthCheckGateWayName, failureStatus: HealthStatus.Degraded);
            services.AddHealthChecks().AddRedis(redisConnectionString: appSetting.RedisServer.Server, name: Constants.DefaultHealthCheckRedisName, failureStatus: HealthStatus.Degraded);
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
    /// SystemCpuHealthcheck
    /// </summary>
    public class SystemCpuHealthcheck : IHealthCheck
    {
        /// <summary>
        /// CheckHealthAsync
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
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

            if (cpuUsageTotal > 90)
            {
                status = HealthStatus.Degraded;
            }
            var result = new HealthCheckResult(status, null, null, data);

            return await Task.FromResult(result);
        }
    }

    /// <summary>
    /// SystemMemoryHealthcheck
    /// </summary>
    public class SystemMemoryHealthcheck : IHealthCheck
    {
        /// <summary>
        /// CheckHealthAsync
        /// </summary>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = new MemoryMetricsClient();
            var metrics = client.GetMetrics();
            var percentUsed = 100 * metrics.Used / metrics.Total;

            var status = HealthStatus.Healthy;
            if (percentUsed > 90)
            {
                status = HealthStatus.Degraded;
            }

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
        /// Total Memory
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Used Memory
        /// </summary>
        public double Used { get; set; }
        /// <summary>
        /// Free Memory
        /// </summary>
        public double Free { get; set; }
    }

    /// <summary>
    /// MemoryMetrics
    /// </summary>
    public class MemoryMetricsClient
    {
        /// <summary>
        /// GetMetrics
        /// </summary>
        public MemoryMetrics GetMetrics()
        {
            var metrics = IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

            return metrics;
        }

        private static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            return isUnix;
        }

        private MemoryMetrics GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                if (process != null) output = process.StandardOutput.ReadToEnd();
            }

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

        private MemoryMetrics GetUnixMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo("free -m")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"free -m\"",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                if (process != null) output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

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
    /// UIHealthReport
    /// </summary>
    public class UiHealthReport
    {
        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        private UIHealthStatus Status { get; set; }

        /// <summary>
        /// TotalDuration
        /// </summary>
        /// <value></value>
        private TimeSpan TotalDuration { get; set; }

        /// <summary>
        /// Entries
        /// </summary>
        /// <value></value>
        private Dictionary<string, UIHealthReportEntry> Entries { get; }

        /// <summary>
        /// UIHealthReport
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="totalDuration"></param>
        private UiHealthReport(Dictionary<string, UIHealthReportEntry> entries, TimeSpan totalDuration)
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
            var uiReport = new UiHealthReport(new Dictionary<string, UIHealthReportEntry>(), report.TotalDuration)
            {
                Status = (UIHealthStatus)report.Status,
            };

            foreach (var (key, value) in report.Entries)
            {
                var entry = new UIHealthReportEntry
                {
                    Data = value.Data,
                    Description = value.Description,
                    Duration = value.Duration,
                    Status = (UIHealthStatus)value.Status
                };

                if (value.Exception != null)
                {
                    var message = value.Exception?
                        .Message
                        .ToString();

                    entry.Exception = message;
                    entry.Description = value.Description ?? message;
                }

                uiReport.Entries.Add(key, entry);
            }

            return uiReport;
        }

        /// <summary>
        /// CreateFrom
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="entryName"></param>
        /// <returns></returns>
        public static UiHealthReport CreateFrom(Exception exception, string entryName = "Endpoint")
        {
            var uiReport = new UiHealthReport(new Dictionary<string, UIHealthReportEntry>(), TimeSpan.FromSeconds(0))
            {
                Status = UIHealthStatus.Unhealthy,
            };

            uiReport.Entries.Add(entryName, new UIHealthReportEntry
            {
                Exception = exception.Message,
                Description = exception.Message,
                Duration = TimeSpan.FromSeconds(0),
                Status = UIHealthStatus.Unhealthy
            });

            return uiReport;
        }
    }
    /// <summary>
    /// /UIHealthStatus
    /// </summary>
    public enum UIHealthStatus
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
    /// UIHealthReportEntry
    /// </summary>
    public class UIHealthReportEntry
    {
        /// <summary>
        /// Data
        /// </summary>
        /// <value></value>
        public IReadOnlyDictionary<string, object> Data { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        /// <value></value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        /// <value></value>
        public string Exception { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <value></value>
        public UIHealthStatus Status { get; set; }
    }
    
    /// <summary>
    /// UIResponseWriter
    /// </summary>
    public static class UiResponseWriter
    {

        private static readonly byte[] EmptyResponse = { (byte)'{', (byte)'}' };
        private static readonly Lazy<JsonSerializerOptions> Options = new(CreateJsonOptions);

        /// <summary>
        /// WriteHealthCheckUIResponse
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        public static async Task WriteHealthCheckUiResponse(HttpContext httpContext, HealthReport report)
        {
            if (report != null)
            {
                httpContext.Response.ContentType = Constants.HeaderJson;

                var uiReport = UiHealthReport
                    .CreateFrom(report);

                await using var responseStream = new MemoryStream();

                await JsonSerializer.SerializeAsync(responseStream, uiReport, Options.Value);
                await httpContext.Response.BodyWriter.WriteAsync(responseStream.ToArray());
            }
            else
            {
                await httpContext.Response.BodyWriter.WriteAsync(EmptyResponse);
            }
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var opt = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };

            opt.Converters.Add(new JsonStringEnumConverter());

            opt.Converters.Add(new TimeSpanConverter());

            return opt;
        }
    }

    internal class TimeSpanConverter
        : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return default;
        }
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
