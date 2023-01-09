// ------------------------------------------------------------------------------------
// PerformanceBehaviour.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;

namespace netca.Application.Common.Behaviours;

/// <summary>
/// PerformanceBehaviour
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceBehaviour{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userAuthorizationService"></param>
    /// <param name="appSetting"></param>
    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUserAuthorizationService userAuthorizationService,
        AppSetting appSetting)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _userAuthorizationService = userAuthorizationService;
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
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= _appSetting.RequestPerformanceInMs)
            return response;

        var requestName = typeof(TRequest).Name;
        var userName = _userAuthorizationService.GetAuthorizedUser().UserName;

        _logger.LogWarning(
            "netca Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserName} {@Request}",
            requestName,
            elapsedMilliseconds,
            userName,
            request
        );

        return response;
    }
}