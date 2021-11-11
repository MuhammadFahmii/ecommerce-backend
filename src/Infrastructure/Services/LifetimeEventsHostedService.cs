// ------------------------------------------------------------------------------------
// LifetimeEventsHostedService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using netca.Infrastructure.Apis;

namespace netca.Infrastructure.Services
{
    /// <summary>
    /// LifetimeEventsHostedService
    /// </summary>
    public class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger<LifetimeEventsHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly AppSetting _appSetting;
        private readonly string _appName;
        private readonly bool _isEnable;

        private const string ImgWarning = Constants.MsTeamsImageWarning;
        private MsTeamTemplate _tmpl;

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeEventsHostedService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appLifetime"></param>
        /// <param name="appSetting"></param>
        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger, IHostApplicationLifetime appLifetime, AppSetting appSetting)
        {
            _logger = logger;
            _appLifetime = appLifetime;
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
            _logger.LogDebug($"Sending message to MsTeam with color {_tmpl.ThemeColor}");
            SendToMsTeams.Send(_appSetting, _tmpl).ConfigureAwait(false);
        }
    }
}