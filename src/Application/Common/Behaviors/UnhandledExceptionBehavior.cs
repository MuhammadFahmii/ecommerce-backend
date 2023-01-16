// ------------------------------------------------------------------------------------
// UnhandledExceptionBehavior.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;

namespace netca.Application.Common.Behaviors;

/// <summary>
/// UnhandledExceptionBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledExceptionBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    public UnhandledExceptionBehavior(ILogger<TRequest> logger, AppSetting appSetting)
    {
        _logger = logger;
        _appSetting = appSetting;
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
                    _logger.LogError(
                        ex,
                        "{namespace} Request: Unhandled Exception for Request {Name} {@Request}",
                        _appSetting.App.Namespace,
                        typeof(TRequest).Name,
                        request);
                    break;
            }

            throw;
        }
    }
}
