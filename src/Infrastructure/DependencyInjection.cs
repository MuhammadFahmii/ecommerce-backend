// ------------------------------------------------------------------------------------
// DependencyInjection.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Infrastructure.Files;
using netca.Infrastructure.Jobs;
using netca.Infrastructure.Persistence;
using netca.Infrastructure.Services;
using netca.Infrastructure.Services.Cache;
using Quartz;

namespace netca.Infrastructure;

/// <summary>
/// DependencyInjection
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// AddInfrastructure
    /// </summary>
    /// <param name="services"></param>
    /// <param name="environment"></param>
    /// <param name="appSetting"></param>
    public static void AddInfrastructure(
        this IServiceCollection services, IWebHostEnvironment? environment, AppSetting appSetting)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlServer(
                    appSetting.ConnectionStrings.DefaultConnection,
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.CommandTimeout(appSetting.DatabaseSettings.CommandTimeout);
                        b.EnableRetryOnFailure(
                            appSetting.DatabaseSettings.MaxRetryCount,
                            TimeSpan.FromSeconds(appSetting.DatabaseSettings.MaxRetryDelay),
                            null
                        );
                        var env = environment?.IsProduction() ?? false;
                        if (!env)
                        {
                            options.EnableSensitiveDataLogging();
                        }
                    }),
            ServiceLifetime.Transient
        );

        services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandlerService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddHostedService<LifetimeEventsHostedService>();
        services.AddHostedService<OrderProcessService>();

        if (!appSetting.BackgroundJob.IsEnable)
            return;

        services.Configure<QuartzOptions>(options =>
        {
            options.Scheduling.IgnoreDuplicates = appSetting.BackgroundJob.PersistentStore.IgnoreDuplicates;
            options.Scheduling.OverWriteExistingData = appSetting.BackgroundJob.PersistentStore.OverWriteExistingData;
            options.Scheduling.ScheduleTriggerRelativeToReplacedTrigger = appSetting.BackgroundJob.PersistentStore
                .ScheduleTriggerRelativeToReplacedTrigger;
        });

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseJobAutoInterrupt(options => { options.DefaultMaxRunTime = TimeSpan.FromMinutes(appSetting.BackgroundJob.DefaultMaxRunTime); });
            q.InterruptJobsOnShutdown = false;
            q.InterruptJobsOnShutdownWithWait = true;
            if (appSetting.BackgroundJob.UsePersistentStore)
            {
                q.SchedulerId = appSetting.App.Title;
                q.SchedulerName = $"{appSetting.App.Title} Scheduler";
                q.MaxBatchSize = 10;
                q.UsePersistentStore(s =>
                {
                    s.UseSqlServer(options =>
                    {
                        options.ConnectionString = appSetting.BackgroundJob.PersistentStore.ConnectionString;
                        options.TablePrefix = appSetting.BackgroundJob.PersistentStore.TablePrefix;
                    });
                    s.RetryInterval = TimeSpan.FromMilliseconds(appSetting.BackgroundJob.PersistentStore.RetryInterval);
                    s.UseJsonSerializer();
                    if (appSetting.BackgroundJob.PersistentStore.UseCluster)
                    {
                        s.UseClustering(cfg =>
                        {
                            cfg.CheckinInterval =
                                TimeSpan.FromMilliseconds(appSetting.BackgroundJob.PersistentStore.CheckinInterval);
                            cfg.CheckinMisfireThreshold =
                                TimeSpan.FromMilliseconds(appSetting.BackgroundJob.PersistentStore.CheckinMisfireThreshold);
                        });
                    }
                });
                q.MisfireThreshold = TimeSpan.FromMilliseconds(appSetting.BackgroundJob.PersistentStore.MisfireThreshold);
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = appSetting.BackgroundJob.PersistentStore.MaxConcurrency;
                });
            }

            q.AddJobAndTrigger<DeleteChangelogJob>(appSetting);
            q.AddJobAndTrigger<ProduceOrderJob>(appSetting);
            q.AddJobAndTrigger<CacheTeamsJob>(appSetting);
        });

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);
    }
}