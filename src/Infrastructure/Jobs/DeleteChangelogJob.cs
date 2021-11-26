// ------------------------------------------------------------------------------------
// DeleteChangelogJob.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using netca.Application.Changelogs.Commands.DeleteChangelog;
using Quartz;

namespace netca.Infrastructure.Jobs
{
    /// <summary>
    /// DeleteChangelogJob
    /// </summary>
    [DisallowConcurrentExecution]
    public class DeleteChangelogJob : IJob
    {
        private readonly ILogger<DeleteChangelogJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteChangelogJob"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public DeleteChangelogJob(ILogger<DeleteChangelogJob> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogDebug("Process delete changelog");
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new DeleteChangelogCommand());
            }
            catch (Exception e)
            {
                _logger.LogError("Error when running worker delete changelog: {message}", e.Message);
            }
            finally
            {
                _logger.LogDebug("Delete changelog done");
            }
        }
    }
}
