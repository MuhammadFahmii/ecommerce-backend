// ------------------------------------------------------------------------------------
// OrderProcessService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;

namespace netca.Infrastructure.Services;

/// <summary>
/// OrderProcessService
/// </summary>
public class OrderProcessService : BaseWorkerService
{
    private readonly ILogger<OrderProcessService> _logger;
    private readonly IRedisService _redisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderProcessService"/> class.
    /// OrderProcessService
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="redisService"></param>
    public OrderProcessService(ILogger<OrderProcessService> logger, IRedisService redisService)
    {
        _logger = logger;
        _redisService = redisService;
    }

    /// <summary>
    /// EHConsumerService is running
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task? ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EHConsumerService is running");
        await RunJob(stoppingToken);
    }

    private Task RunJob(CancellationToken cancellationToken)
    {
        return Task.Run(
            () =>
            {
                Task.Run(
                    async () =>
                    {
                        try
                        {
                            await OrderProcessing(cancellationToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Error when processing order : {Message}", e.Message);
                        }
                    }, cancellationToken);
            }, cancellationToken);
    }

    /// <summary>
    /// OrderProcessing
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task OrderProcessing(CancellationToken cancellationToken)
    {
        do
        {
            var data = await _redisService.ListLeftPopAsync("order");
            if (string.IsNullOrEmpty(data))
                continue;
            _logger.LogInformation("{Date} -> receiving order {D}", DateTime.UtcNow, data);
            Thread.Sleep(100);
        } while (!cancellationToken.IsCancellationRequested);
    }
}