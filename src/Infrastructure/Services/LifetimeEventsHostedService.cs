// ------------------------------------------------------------------------------------
// LifetimeEventsHostedService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using netca.Infrastructure.Apis;
using Quartz;
using Quartz.Impl.Matchers;

namespace netca.Infrastructure.Services;

/// <summary>
/// LifetimeEventsHostedService
/// </summary>
public class LifetimeEventsHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<LifetimeEventsHostedService> _logger;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly AppSetting _appSetting;
    private readonly string _appName;
    private readonly bool _isEnable;
    private const string ImgWarning = Constants.MsTeamsImageWarning;
    private MsTeamTemplate? _tmpl;

    /// <summary>
    /// Initializes a new instance of the <see cref="LifetimeEventsHostedService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appLifetime"></param>
    /// <param name="schedulerFactory"></param>
    /// <param name="appSetting"></param>
    public LifetimeEventsHostedService(
        ILogger<LifetimeEventsHostedService> logger,
        IHostApplicationLifetime appLifetime,
        ISchedulerFactory schedulerFactory,
        AppSetting appSetting)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _schedulerFactory = schedulerFactory;
        _appSetting = appSetting;

        _isEnable = _appSetting.Bot.IsEnable;
        _appName = $"[{_appSetting.Bot.ServiceName}](http://{_appSetting.Bot.ServiceDomain})";
    }

    /// <summary>
    /// StartAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(CleanUpDisableJobQuartz);
        _appLifetime.ApplicationStopping.Register(CleanUpQuartz);

        if (!_isEnable)
            return Task.CompletedTask;

        _appLifetime.ApplicationStarted.Register(OnStarted);
        _appLifetime.ApplicationStopping.Register(OnStopping);
        _appLifetime.ApplicationStopped.Register(OnStopped);

        return Task.CompletedTask;
    }

    /// <summary>
    /// StopAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void CleanUpDisableJobQuartz()
    {
        if (!_appSetting.BackgroundJob.IsEnable || !_appSetting.BackgroundJob.UsePersistentStore)
            return;

        var removeJobs = new List<JobKey>();

        var jobs = _appSetting.BackgroundJob.Jobs
            .Where(x => !x.IsEnable)
            .ToList();

        foreach (var job in jobs)
        {
            var jobName = job.Name;

            if (job.Parameters == null || job.Parameters.Count == 0)
            {
                removeJobs.Add(new JobKey(jobName, $"{jobName}_Group"));
            }
            else
            {
                for (byte i = 0; i < job.Parameters.Count; i++)
                    removeJobs.Add(new JobKey($"{jobName}{i}", $"{jobName}_Group"));
            }
        }

        var scheduler = _schedulerFactory.GetScheduler().Result;

        scheduler.DeleteJobs(removeJobs);
    }

    private void CleanUpQuartz()
    {
        if (!_appSetting.BackgroundJob.IsEnable || !_appSetting.BackgroundJob.UsePersistentStore)
            return;

        var scheduler = _schedulerFactory.GetScheduler().Result;

        var triggers = (
                from jobGroupName in scheduler.GetTriggerGroupNames().Result
                from triggerKey in scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(jobGroupName)).Result
                select scheduler.GetTrigger(triggerKey).Result)
            .ToList();

        var jobs = triggers
            .Where(x => x!.Key.Name.Contains(_appSetting.BackgroundJob.HostName))
            .Select(x => x!.JobKey)
            .ToList();

        scheduler.DeleteJobs(jobs);
    }

    private void OnStarted()
    {
        const string message = Constants.MsTeamsactivitySubtitleStart;

        _logger.LogDebug(message);

        _tmpl = new MsTeamTemplate();

        var sections = new List<Section>();
        var facts = new List<Fact>
        {
            new() { Name = "Date", Value = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}" },
            new() { Name = "Message", Value = message }
        };

        sections.Add(new Section
        {
            ActivityTitle = $"{_appName}",
            ActivitySubtitle = message,
            Facts = facts,
            ActivityImage = ImgWarning
        });

        _tmpl = _tmpl with
        {
            Summary = $"{_appName} has started",
            ThemeColor = Constants.MsTeamsThemeColorWarning,
            Sections = sections
        };

        Send();
    }

    private void OnStopping()
    {
        const string message = Constants.MsTeamsactivitySubtitleStop;

        _logger.LogDebug("Try to stopping Application");

        _tmpl = new MsTeamTemplate();

        var sections = new List<Section>();
        var facts = new List<Fact>
        {
            new() { Name = "Date", Value = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}" },
            new() { Name = "Message", Value = message }
        };

        sections.Add(new Section
        {
            ActivityTitle = $"{_appName}",
            ActivitySubtitle = message,
            Facts = facts,
            ActivityImage = ImgWarning
        });

        _tmpl = _tmpl with
        {
            Summary = $"{_appName} has stopping",
            ThemeColor = Constants.MsTeamsThemeColorWarning,
            Sections = sections
        };

        Send();
    }

    private void OnStopped()
    {
        _logger.LogDebug("Application stopped");
    }

    private void Send()
    {
        _logger.LogDebug("Sending message to MsTeam with color {color}", _tmpl?.ThemeColor);

        if (_tmpl != null)
        	SendToMsTeams.Send(_appSetting, _tmpl).ConfigureAwait(false);
    }
}