// ------------------------------------------------------------------------------------
// QuartzService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using netca.Application.Common.Models;
using Quartz;

namespace netca.Infrastructure.Services
{
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
        /// <exception cref="Exception"></exception>
        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator quartz,
            AppSetting appSetting)
            where T : IJob
        {
            var jobName = typeof(T).Name;
            var cronSchedule = appSetting.BackgroundJob.Jobs.FirstOrDefault(x => x.Key == jobName).Value;


            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new ArgumentNullException($"No Quartz.NET Cron schedule found for {jobName} in configuration");
            }
            
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}