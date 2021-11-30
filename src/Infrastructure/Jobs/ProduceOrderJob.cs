// ------------------------------------------------------------------------------------
// ProduceOrderJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using Quartz;

namespace netca.Infrastructure.Jobs
{
    /// <summary>
    /// ProduceOrderJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class ProduceOrderJob : IJob
    {
        private readonly ILogger<ProduceOrderJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProduceOrderJob"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public ProduceOrderJob(ILogger<ProduceOrderJob> logger, IServiceScopeFactory serviceScopeFactory)
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
                var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
                var dt = scope.ServiceProvider.GetRequiredService<IDateTime>();
                for (var index = 1; index <= 100; index++)
                {
                    redisService.ListLeftPushAsync("order", "{ 'order': " + index + "}");
                    _logger.LogWarning("{now} -> sending order {i}", dt.Now, index);
                    Thread.Sleep(100);
                }
            }

            return Task.CompletedTask;
        }
    }
}
