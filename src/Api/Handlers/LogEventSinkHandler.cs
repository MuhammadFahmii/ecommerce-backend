// ------------------------------------------------------------------------------------
// LogEventSinkHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using netca.Application.Common.Models;
using netca.Infrastructure.Apis;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Constants = netca.Application.Common.Models.Constants;
using ILogger = Serilog.ILogger;

namespace netca.Api.Handlers
{
    /// <summary>
    /// LogEventSinkHandler
    /// </summary>
    public class LogEventSinkHandler : ILogEventSink
    {
        private readonly AppSetting _appSetting;
        private readonly IMemoryCache _memoryCache;
        private static readonly ILogger Logger = Log.ForContext(typeof(LogEventSinkHandler));

        /// <summary>
        /// LogEventSinkHandler
        /// </summary>
        /// <param name="appSetting"></param>
        /// <param name="memoryCache"></param>

        public LogEventSinkHandler(AppSetting appSetting, IMemoryCache memoryCache)
        {
            _appSetting = appSetting;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Emit
        /// </summary>
        /// <param name="logEvent"></param>
        public void Emit(LogEvent logEvent)
        {
            if (!_appSetting.Bot.IsEnable) return;
            if (!logEvent.Level.Equals(LogEventLevel.Error)) return;
            var cacheMsTeam = GetCounter();
            var hours = (DateTime.UtcNow - cacheMsTeam.Date).TotalHours;
            if (cacheMsTeam.Counter >= _appSetting.Bot.CacheMSTeam.Counter || hours >= _appSetting.Bot.CacheMSTeam.Hours) return;
            SetCounter(cacheMsTeam); 
            var facts = new List<Fact>();
            var sections = new List<Section>();
            var serviceName = _appSetting.Bot.ServiceName;
            var serviceDomain = _appSetting.Bot.ServiceDomain;
            facts.Add(new Fact { name = "Date", value = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}" });
            var app = $"[{serviceName}](http://{serviceDomain})";
            sections.Add(new Section
            {
                activityTitle = app,
                activitySubtitle = "Internal Server Error",
                Facts = facts,
                activityImage = Constants.MsTeamsImageError
            });
            facts.Add(new Fact { name = "Message", value = logEvent.RenderMessage() });
            var tmpl = new MsTeamTemplate
            {
                sections = sections,
                summary = $"{Constants.MsTeamsSummaryError} with {app}"
            };
            Logger.Debug($"Sending message to MsTeam with color {tmpl.themeColor}");
            SendToMsTeams.Send(_appSetting, tmpl).ConfigureAwait(false);
        }

        private void SetCounter(CacheMSTeam cacheMsTeam)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()  
                .SetAbsoluteExpiration(TimeSpan.FromDays(2));
            _memoryCache.Set("CacheMSTeams", cacheMsTeam, cacheEntryOptions);
        }
        
        private CacheMSTeam GetCounter()
        {
            var isExist = _memoryCache.TryGetValue("CacheMSTeams", out CacheMSTeam cacheMsTeam);
            if (isExist)
            {
                return cacheMsTeam;
            }

            cacheMsTeam = new CacheMSTeam{Counter = 0, Date = DateTime.UtcNow};
            return cacheMsTeam;
        }
    }
}
