// ------------------------------------------------------------------------------------
// UnhandledExceptionBehaviour.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Exceptions;

namespace netca.Application.Common.Behaviours
{
    /// <summary>
    /// UnhandledExceptionBehaviour
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
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
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case OperationCanceledException:
                        _logger.LogWarning("The request has been canceled");
                        break;
                    case ValidationException:
                    case BadRequestException:
                    case NotFoundException:
                        break;
                    default:
                        var requestName = typeof(TRequest).Name;
                        _logger.LogWarning(ex, "netca Request: Unhandled Exception for Request {Name} {Request}", requestName, request);
                        break;
                }

                throw;
            }
        }
    }
}