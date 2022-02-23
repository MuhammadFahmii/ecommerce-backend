// ------------------------------------------------------------------------------------
// QuartzService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using netca.Application.Common.Models;
using Quartz;

namespace netca.Infrastructure.Services;

/// <summary>
/// QuartzService
/// </summary>
public static class QuartzService
{
    /// <summary>
    /// AddJobAndTrigger
    /// </summary>
    /// <param name="quartz"></param>
    /// <param name="appSetting"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="Exception">Exception</exception>
    public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, AppSetting appSetting)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var job = appSetting.BackgroundJob.Jobs.FirstOrDefault(x => x.Name.Equals(jobName));

        if (job is { IsEnable: false })
            return;

        if (job == null || string.IsNullOrEmpty(job.Schedule))
            throw new ArgumentNullException($"No Quartz.NET Cron schedule found for {jobName} in configuration");
        var group = jobName + "Group";
        if (job.IsParallel && appSetting.BackgroundJob.UsePersistentStore)
        {
            jobName += Environment.GetEnvironmentVariable("hostname");
        }

        var jobKey = new JobKey(jobName, group);

        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithDescription(job.Description)
            .WithIdentity(jobName + "trigger", group)
            .WithCronSchedule(job.Schedule, x =>
            {
                if (job.IgnoreMisfire)
                    x.WithMisfireHandlingInstructionDoNothing();
            }));
    }
}