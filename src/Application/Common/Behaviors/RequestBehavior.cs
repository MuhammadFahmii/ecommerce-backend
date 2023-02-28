// ------------------------------------------------------------------------------------
// RequestBehavior.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ecommerce.Application.Common.Exceptions;

namespace ecommerce.Application.Common.Behaviors;

/// <summary>
/// RequestBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RequestBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public RequestBehavior(ILogger<RequestBehavior<TRequest, TResponse>> logger)
    {
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
        var requestType = typeof(TRequest).Name;

        var response = await next();

        if (requestType.EndsWith("Command"))
        {
            _logger.LogDebug("Command Request: {request}", request);
        }
        else if (requestType.EndsWith("Query"))
        {
            _logger.LogDebug("Query Request: {request}", request);
            _logger.LogDebug("Query Response: {response}", response);
        }
        else
        {
            throw new ThrowException("The request is not the Command or Query type");
        }

        return response;
    }
}
