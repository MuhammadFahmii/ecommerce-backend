// ------------------------------------------------------------------------------------
// CacheTeamsJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using Quartz;

namespace netca.Infrastructure.Jobs
{
    /// <summary>
    /// CacheTeamsJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class CacheTeamsJob : IJob
    {
        private readonly ILogger<CacheTeamsJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTeamsJob"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public CacheTeamsJob(ILogger<CacheTeamsJob> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var ch = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                var appSet = scope.ServiceProvider.GetRequiredService<AppSetting>();
                var ctm = GetCounter(ch);
                var hours = (DateTime.UtcNow - ctm.Date).TotalHours;
                if (hours < appSet.Bot.CacheMSTeam.Hours)
                    return Task.CompletedTask;
                _logger.LogWarning($"Resetting CacheMSTeams");
                ctm.Counter = 0;
                ctm.Date = DateTime.UtcNow;
                SetCounter(ctm, ch);
            }

            return Task.CompletedTask;
        }

        private static void SetCounter(CacheMSTeam cacheMsTeam, IMemoryCache mc)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(2));
            mc.Set("CacheMSTeams", cacheMsTeam, cacheEntryOptions);
        }

        private static CacheMSTeam GetCounter(IMemoryCache mc)
        {
            var isExist = mc.TryGetValue("CacheMSTeams", out CacheMSTeam cacheMsTeam);
            if (isExist)
            {
                return cacheMsTeam;
            }

            cacheMsTeam = new CacheMSTeam { Counter = 0, Date = DateTime.UtcNow };
            return cacheMsTeam;
        }
    }
}
