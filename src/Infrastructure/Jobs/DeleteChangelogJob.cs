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
using Serilog;

namespace netca.Infrastructure.Jobs;

/// <summary>
/// DeleteChangelogJob
/// </summary>
public class DeleteChangelogJob : BaseJob<DeleteChangelogJob>
{
    /// <summary>
    /// DeleteChangelogJob
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceScopeFactory"></param>
    public DeleteChangelogJob(ILogger<DeleteChangelogJob> logger, IServiceScopeFactory serviceScopeFactory) : base(logger,
        serviceScopeFactory)
    {
    }

    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task Execute(IJobExecutionContext context)
    {
        try
        {
            Logger.LogDebug("Process delete changelog");
            using var scope = ServiceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new DeleteChangelogCommand());
        }
        catch (Exception e)
        {
            Logger.LogError("Error when running worker delete changelog: {Message}", e.Message);
        }
        finally
        {
            Logger.LogDebug("Delete changelog done");
        }
    }
}