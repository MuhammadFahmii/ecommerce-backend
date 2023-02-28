// ------------------------------------------------------------------------------------
// BaseJob.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ecommerce.Infrastructure.Jobs;

/// <summary>
/// BaseJob
/// </summary>
[DisallowConcurrentExecution]
public abstract class BaseJob<T> : IJob
{
    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger<T> Logger;
    /// <summary>
    /// ServiceScopeFactory
    /// </summary>
    protected readonly IServiceScopeFactory ServiceScopeFactory;
    
    /// <summary>
    /// BaseJob
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceScopeFactory"></param>
    protected BaseJob(ILogger<T> logger, IServiceScopeFactory serviceScopeFactory)
    {
        Logger = logger;
        ServiceScopeFactory = serviceScopeFactory;
    }
    
    /// <summary>
    /// Execute
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task Execute(IJobExecutionContext context)
    {
        Logger.LogDebug("Executing job...");
        return Task.CompletedTask;
    }
}