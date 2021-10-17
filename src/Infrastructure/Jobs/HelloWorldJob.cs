// ------------------------------------------------------------------------------------
// HelloWorldJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        /// <summary>
        /// HelloWorldJob
        /// </summary>
        /// <param name="logger"></param>
        public HelloWorldJob(ILogger<HelloWorldJob> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world!");
            return Task.CompletedTask;
        }
    }
}
