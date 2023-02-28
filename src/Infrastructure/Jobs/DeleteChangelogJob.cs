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
using ecommerce.Application.Changelogs.Commands.DeleteChangelog;
using Quartz;

namespace ecommerce.Infrastructure.Jobs;

/// <summary>
/// DeleteChangelogJob
/// </summary>
public class DeleteChangelogJob : BaseJob<DeleteChangelogJob>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteChangelogJob"/> class.
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    /// <param name="logger"></param>
    public DeleteChangelogJob(IServiceScopeFactory serviceScopeFactory, ILogger<DeleteChangelogJob> logger)
        : base(logger, serviceScopeFactory)
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
            Logger.LogError("Error when running worker delete changelog: {message}", e.Message);
        }
        finally
        {
            Logger.LogDebug("Delete changelog done");
        }
    }
}