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

namespace netca.Infrastructure.Services
{
    /// <summary>
    /// LifetimeEventsHostedService
    /// </summary>
    public class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger<LifetimeEventsHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ISchedulerFactory _iSchedulerFactory;
        private readonly AppSetting _appSetting;
        private readonly string _appName;
        private readonly bool _isEnable;
        private const string ImgWarning = Constants.MsTeamsImageWarning;
        private MsTeamTemplate _tmpl = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeEventsHostedService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appLifetime"></param>
        /// <param name="appSetting"></param>
        /// <param name="iSchedulerFactory"></param>
        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger, IHostApplicationLifetime appLifetime, AppSetting appSetting, ISchedulerFactory iSchedulerFactory)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _appSetting = appSetting;
            _isEnable = _appSetting.Bot.IsEnable;
            _appName = $"[{_appSetting.Bot.ServiceName}](http://{_appSetting.Bot.ServiceDomain})";
            _iSchedulerFactory = iSchedulerFactory;
        }

        /// <summary>
        /// StartAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
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

        private void CleanUpQuartz()
        {
            if (!_appSetting.BackgroundJob.IsEnable || !_appSetting.BackgroundJob.UsePersistentStore)
                return;
            var sc = _iSchedulerFactory.GetScheduler().Result;
            var triggers = (from jobGroupName in sc.GetTriggerGroupNames().Result
                from triggerKey in sc.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(jobGroupName)).Result
                select sc.GetTrigger(triggerKey).Result).ToList();
            var hostname = Environment.GetEnvironmentVariable("hostname");
            var jobs = triggers.Where(x => x!.JobKey.Name.Contains(hostname!)).Select(x => x!.JobKey ).ToList();
            sc.DeleteJobs(jobs);
        }

        private void OnStarted()
        {
            const string msg = Constants.MsTeamsactivitySubtitleStart;
            _logger.LogDebug(msg);

            _tmpl = new MsTeamTemplate();

            var sections = new List<Section>();
            var facts = new List<Fact>
            {
                new() { Name = "Date", Value = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}" },
                new() { Name = "Message", Value = msg }
            };

            sections.Add(new Section
            {
                ActivityTitle = $"{_appName}",
                ActivitySubtitle = msg,
                Facts = facts,
                ActivityImage = ImgWarning
            });

            _tmpl.Summary = $"{_appName} has started";
            _tmpl.ThemeColor = Constants.MsTeamsThemeColorWarning;
            _tmpl.Sections = sections;
            Send();
        }

        private void OnStopping()
        {
            const string msg = Constants.MsTeamsactivitySubtitleStop;
            _logger.LogDebug("Try to stopping Application");

            _tmpl = new MsTeamTemplate();

            var sections = new List<Section>();
            var facts = new List<Fact>
            {
                new() { Name = "Date", Value = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}" },
                new() { Name = "Message", Value = msg }
            };

            sections.Add(new Section
            {
                ActivityTitle = $"{_appName}",
                ActivitySubtitle = msg,
                Facts = facts,
                ActivityImage = ImgWarning
            });
            _tmpl.Summary = $"{_appName} has stopping";
            _tmpl.ThemeColor = Constants.MsTeamsThemeColorWarning;
            _tmpl.Sections = sections;

            Send();
        }

        private void OnStopped()
        {
            _logger.LogDebug("Application stopped");
        }

        private void Send()
        {
            _logger.LogDebug("Sending message to MsTeam with color {color}", _tmpl.ThemeColor);
            SendToMsTeams.Send(_appSetting, _tmpl).ConfigureAwait(false);
        }
    }
}