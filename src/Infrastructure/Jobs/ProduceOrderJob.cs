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

namespace netca.Infrastructure.Jobs;
    /// <summary>
    /// ProduceOrderJob
    /// </summary>
    public class ProduceOrderJob : BaseJob<ProduceOrderJob>
    {
        /// <summary>
        /// ProduceOrderJob
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public ProduceOrderJob(ILogger<ProduceOrderJob> logger, IServiceScopeFactory serviceScopeFactory) : base(logger,
            serviceScopeFactory)
        {
        }

    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task Execute(IJobExecutionContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var data = context.Trigger;
            // var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
            var dt = scope.ServiceProvider.GetRequiredService<IDateTime>();
            for (var index = 1; index <= 1; index++)
            {
               // redisService.ListLeftPushAsync("order", "{ 'order': " + index + "}");
                Logger.LogWarning("{Now} -> sending order {I}", dt.Now, data.Key.Name);
                Thread.Sleep(100);
            }
        }

        return Task.CompletedTask;
    }
}