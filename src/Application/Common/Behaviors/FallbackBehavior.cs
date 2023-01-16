// ------------------------------------------------------------------------------------
// FallbackBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using Polly;

namespace netca.Application.Common.Behaviors;

/// <summary>
/// Wraps request handler execution of requests
/// inside a policy to handle transient fallback the execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class FallbackBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IFallbackHandler<TRequest, TResponse>> _fallbackHandlers;
    private readonly ILogger<FallbackBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FallbackBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="fallbackHandlers"></param>
    /// <param name="logger"></param>
    public FallbackBehavior(
        IEnumerable<IFallbackHandler<TRequest, TResponse>> fallbackHandlers,
        ILogger<FallbackBehavior<TRequest, TResponse>> logger)
    {
        _fallbackHandlers = fallbackHandlers;
        _logger = logger;
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
        var fallbackHandler = _fallbackHandlers.FirstOrDefault();

        if (fallbackHandler == null)
            return await next();

        var requestName = typeof(TRequest).Name;

        return await Policy<TResponse>
            .Handle<Exception>()
            .FallbackAsync(async (cancellationToken) =>
            {
                _logger.LogInformation("Falling back response for request {name}", requestName);

                return await fallbackHandler.HandleFallback(request, cancellationToken).ConfigureAwait(false);
            })
            .ExecuteAsync(() => next());
    }
}