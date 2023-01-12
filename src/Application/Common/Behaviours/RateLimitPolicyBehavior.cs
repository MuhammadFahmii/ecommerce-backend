// ------------------------------------------------------------------------------------
// RateLimitPolicyBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Reflection;
using MediatR;
using netca.Application.Common.Models;
using Polly.RateLimit;

namespace netca.Application.Common.Behaviours;

/// <summary>
/// Applies a rate limit policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RateLimitPolicyAttribute : Attribute
{
    private int _numberOfAllowedExecutions = 100;
    private int _durationLimit = 1;
    private int _burst = 1;

    /// <summary>
    /// Gets or sets the amount of times execution allowed per duration.
    /// Defaults to 100 times.
    /// </summary>
    public int NumberOfAllowedExecutions
    {
        get => _numberOfAllowedExecutions;
        set
        {
            if (value < 1)
                throw new ArgumentException("Number Of Executions must be higher than 1.", nameof(value));

            _numberOfAllowedExecutions = value;
        }
    }

    /// <summary>
    /// Gets or sets the duration limit in seconds.
    /// Defaults to 1 seconds.
    /// </summary>
    public int DurationLimit
    {
        get => _durationLimit;
        set
        {
            if (value < 1)
                throw new ArgumentException("Duration limit must be higher than 1 second.", nameof(value));

            _durationLimit = value;
        }
    }

    /// <summary>
    /// Gets or sets the burst.
    /// Defaults to 1.
    /// </summary>
    public int Burst
    {
        get => _burst;
        set
        {
            if (value < 1)
                throw new ArgumentException("Burst value must be higher than 1.", nameof(value));

            _burst = value;
        }
    }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="RateLimitPolicyAttribute"/>
/// inside a policy to handle transient rate limit the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RateLimitPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly RateLimit? _rateLimitPolicy;

    private AsyncRateLimitPolicy<TResponse>? _rateLimit;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitPolicyBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="appSetting"></param>
    public RateLimitPolicyBehavior(AppSetting appSetting)
    {
        _rateLimitPolicy = appSetting?.ResiliencyPolicy?.RateLimit;
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
        var rateLimitAttr = typeof(TRequest).GetCustomAttribute<RateLimitPolicyAttribute>();

        if (rateLimitAttr == null && !(_rateLimitPolicy?.Enabled ?? true))
            return await next();

        _rateLimit ??= Polly.Policy.RateLimitAsync<TResponse>(
            rateLimitAttr?.NumberOfAllowedExecutions ?? _rateLimitPolicy!.NumberOfAllowedExecutions,
            TimeSpan.FromSeconds(rateLimitAttr?.DurationLimit ?? _rateLimitPolicy!.DurationLimit),
            rateLimitAttr?.Burst ?? _rateLimitPolicy!.Burst);

        return await _rateLimit.ExecuteAsync(() => next());
    }
}