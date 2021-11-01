// ------------------------------------------------------------------------------------
// HelloWorldJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using Quartz;

namespace netca.Infrastructure.Jobs
{
    /// <summary>
    /// HelloWorldJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class HelloWorldJob : IJob
    {
        private readonly ILogger<HelloWorldJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;  
        /// <summary>
        /// HelloWorldJob
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public HelloWorldJob(ILogger<HelloWorldJob> logger, IServiceScopeFactory serviceScopeFactory)
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
                var dt = scope.ServiceProvider.GetRequiredService<IDateTime>();
                _logger.LogWarning($"Hello world! at {dt.Now}");
            }
            
            return Task.CompletedTask;
        }
    }
}
