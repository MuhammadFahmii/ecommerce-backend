// ------------------------------------------------------------------------------------
// CircuitBreakerPolicyBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;
using ecommerce.Application.Common.Exceptions;
using ecommerce.Application.Common.Models;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.RateLimit;
using Polly.Timeout;

namespace ecommerce.Application.Common.Behaviors;

/// <summary>
/// Applies a circuit breaker policy on the MediatR request.
/// Apply this attribute to the MediatR <see cref="IRequest"/> class (not on the handler).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CircuitBreakerPolicyAttribute : Attribute
{
    private int _exceptionAllowed = 3;
    private int _durationOfBreak = 30;

    /// <summary>
    /// Gets or sets the amount of times to exception allowed the execution.
    /// Defaults to 3 times.
    /// </summary>
    public int ExceptionAllowed
    {
        get => _exceptionAllowed;
        set
        {
            if (value < 1)
                throw new ArgumentException("ExceptionAllowed must be higher than 1.", nameof(value));

            _exceptionAllowed = value;
        }
    }

    /// <summary>
    /// Gets or sets the duration of break in seconds.
    /// Defaults to 30 seconds.
    /// </summary>
    public int DurationOfBreak
    {
        get => _durationOfBreak;
        set
        {
            if (value < 1)
                throw new ArgumentException("Duration of break must be higher than 1 second.", nameof(value));

            _durationOfBreak = value;
        }
    }

    /// <summary>
    /// Gets or sets the handle type.
    /// </summary>
    public Type? HandleType { get; set; }
}

/// <summary>
/// Wraps request handler execution of requests decorated with the <see cref="CircuitBreakerPolicyAttribute"/>
/// inside a policy to handle transient failures and circuit breaker the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class CircuitBreakerPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<CircuitBreakerPolicyBehavior<TRequest, TResponse>> _logger;
    private readonly CircuitBreaker _circuitBreakerPolicy;
    private readonly string _requestName;

    private AsyncCircuitBreakerPolicy<TResponse>? _circuitBreaker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CircuitBreakerPolicyBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    public CircuitBreakerPolicyBehavior(
        ILogger<CircuitBreakerPolicyBehavior<TRequest, TResponse>> logger, AppSetting appSetting)
    {
        _logger = logger;
        _circuitBreakerPolicy = appSetting?.ResiliencyPolicy?.CircuitBreaker!;
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
        var circuitBreakerAttr = typeof(TRequest).GetCustomAttribute<CircuitBreakerPolicyAttribute>();

        if (circuitBreakerAttr == null && !_circuitBreakerPolicy.Enabled)
            return await next();

        _circuitBreaker ??= Policy<TResponse>
            .Handle<Exception>(ex =>
            {
                if (ex.GetType() == circuitBreakerAttr?.HandleType)
                    return true;

                return ex switch
                {
                    BulkheadRejectedException or
                    RateLimitRejectedException or
                    TimeoutRejectedException or
                    OperationCanceledException or
                    ValidationException or
                    BadRequestException or
                    NotFoundException => false,
                    _ => true,
                };
            })
            .CircuitBreakerAsync(
                circuitBreakerAttr?.ExceptionAllowed ?? _circuitBreakerPolicy.ExceptionAllowed,
                TimeSpan.FromSeconds(circuitBreakerAttr?.DurationOfBreak ?? _circuitBreakerPolicy.DurationOfBreak),
                OnBreak,
                OnReset);

        return await _circuitBreaker.ExecuteAsync(() => next());
    }

    private void OnBreak(DelegateResult<TResponse> _, TimeSpan timeSpan)
    {
        _logger.LogInformation(
            "Circuit breaker open for request {name}, reset in {time} seconds",
            _requestName,
            timeSpan.Seconds);
    }

    private void OnReset()
    {
        _logger.LogInformation("Resetting Circuit breaker for request {name}", _requestName);
    }
}