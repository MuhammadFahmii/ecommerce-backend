// ------------------------------------------------------------------------------------
// BulkheadPolicyBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using Polly.Bulkhead;

namespace netca.Application.Common.Behaviours;

/// <summary>
/// Applies a fallback policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BulkheadPolicyAttribute : Attribute
{
    private int _maxParallelization = 100;
    private int _maxQueuingActions = 20;

    /// <summary>
    /// Gets or sets the max parallelization.
    /// Defaults to 100 parallel.
    /// </summary>
    public int MaxParallelization
    {
        get => _maxParallelization;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Max parallelization count must be higher than 1.", nameof(value));
            }

            _maxParallelization = value;
        }
    }

    /// <summary>
    /// Gets or sets the max queuing actions.
    /// Defaults to 20 queues.
    /// </summary>
    public int MaxQueuingActions
    {
        get => _maxQueuingActions;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Max queuing actions count must be higher than 1.", nameof(value));
            }

            _maxQueuingActions = value;
        }
    }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="BulkheadPolicyAttribute"/>
/// inside a policy to handle transient bulk head the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class BulkheadPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<BulkheadPolicyBehavior<TRequest, TResponse>> _logger;
    private readonly Bulkhead _bulkHeadPolicy;
    private readonly string _requestName;

    private AsyncBulkheadPolicy<TResponse>? _bulkHead;

    /// <summary>
    /// Initializes a new instance of the <see cref="BulkheadPolicyBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    public BulkheadPolicyBehavior(
        ILogger<BulkheadPolicyBehavior<TRequest, TResponse>> logger, AppSetting appSetting)
    {
        _logger = logger;
        _bulkHeadPolicy = appSetting?.ResiliencyPolicy?.Bulkhead!;
        _requestName = typeof(TRequest).Name;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var bulkHeadAttr = typeof(TRequest).GetCustomAttribute<BulkheadPolicyAttribute>();

        if (bulkHeadAttr == null && !_bulkHeadPolicy.Enabled)
            return await next();

        _bulkHead ??= Polly.Policy.BulkheadAsync<TResponse>(
            bulkHeadAttr?.MaxParallelization ?? _bulkHeadPolicy.MaxParallelization,
            bulkHeadAttr?.MaxQueuingActions ?? _bulkHeadPolicy.MaxQueuingActions,
            (_) =>
            {
                _logger.LogInformation("Bulkhead limit reached for request {name}", _requestName);
                return Task.CompletedTask;
            });

        return await _bulkHead.ExecuteAsync(() => next());
    }
}