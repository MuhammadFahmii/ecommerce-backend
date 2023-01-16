// ------------------------------------------------------------------------------------
// TimeoutPolicyBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using Polly.Timeout;
using Timeout = netca.Application.Common.Models.Timeout;

namespace netca.Application.Common.Behaviors;

/// <summary>
/// Applies a timeout policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TimeoutPolicyAttribute : Attribute
{
    private int _duration = 180;

    /// <summary>
    /// Gets or sets the timeout duration of the execution.
    /// Defaults to 180 seconds.
    /// </summary>
    public int Duration
    {
        get => _duration;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Duration must be higher than 1 seconds.", nameof(value));
            }

            _duration = value;
        }
    }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="TimeoutPolicyAttribute"/>
/// inside a policy to handle transient timeout policy of the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class TimeoutPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TimeoutPolicyBehavior<TRequest, TResponse>> _logger;
    private readonly Timeout _timeoutPolicy;
    private readonly string _requestName;

    private AsyncTimeoutPolicy<TResponse>? _timeout;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeoutPolicyBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    public TimeoutPolicyBehavior(
        ILogger<TimeoutPolicyBehavior<TRequest, TResponse>> logger, AppSetting appSetting)
    {
        _logger = logger;
        _timeoutPolicy = appSetting?.ResiliencyPolicy?.Timeout!;
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
        var timeoutAttr = typeof(TRequest).GetCustomAttribute<TimeoutPolicyAttribute>();

        if (timeoutAttr == null && !_timeoutPolicy.Enabled)
            return await next();

        _timeout ??= Polly.Policy.TimeoutAsync<TResponse>(
            timeoutAttr?.Duration ?? _timeoutPolicy.Duration,
            TimeoutStrategy.Pessimistic,
            (_, _, _, _) =>
            {
                _logger.LogInformation("Timeout reached for request {name}", _requestName);
                return Task.CompletedTask;
            });

        return await _timeout.ExecuteAsync(() => next());
    }
}