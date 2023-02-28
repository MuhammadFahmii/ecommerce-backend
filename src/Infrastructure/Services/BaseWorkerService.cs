// ------------------------------------------------------------------------------------
// BaseWorkerService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ecommerce.Infrastructure.Services;

/// <summary>
/// BaseWorkerService
/// </summary>
public abstract class BaseWorkerService : IHostedService, IDisposable
{
    private Task? _executingTask;
    private readonly CancellationTokenSource _stoppingCts = new();
    private bool _disposedValue;

    /// <summary>
    /// ExecuteAsync
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected abstract Task? ExecuteAsync(CancellationToken stoppingToken);

    /// <summary>
    /// StartAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _executingTask = ExecuteAsync(_stoppingCts.Token);

        return _executingTask!.IsCompleted ? _executingTask : Task.CompletedTask;
    }

    /// <summary>
    /// StopAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _stoppingCts.Cancel();
        }
        finally
        {
            await Task.WhenAny(_executingTask!, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
            _stoppingCts.Cancel();

        _disposedValue = true;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}