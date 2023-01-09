// ------------------------------------------------------------------------------------
// RequestBehaviour.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Exceptions;

namespace netca.Application.Common.Behaviours;

/// <summary>
/// RequestBehaviour
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RequestBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RequestBehaviour<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBehaviour{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public RequestBehaviour(ILogger<RequestBehaviour<TRequest, TResponse>> logger)
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
            _logger.LogDebug("Command Request: {R}", request);
        }
        else if (requestType.EndsWith("Query"))
        {
            _logger.LogDebug("Query Request: {R}", request);
            _logger.LogDebug("Query Response: {R}", request);
        }
        else
        {
            throw new ThrowException("The request is not the Command or Query type");
        }

        return response;
    }
}