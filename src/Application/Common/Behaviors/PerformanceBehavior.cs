// ------------------------------------------------------------------------------------
// PerformanceBehavior.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Application.Common.Models;

namespace ecommerce.Application.Common.Behaviors;

/// <summary>
/// PerformanceBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userAuthorizationService"></param>
    /// <param name="appSetting"></param>
    public PerformanceBehavior(
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
        var user = _userAuthorizationService.GetAuthorizedUser();
        var userName = user.UserName ?? Constants.SystemName;

        _logger.LogWarning(
            "{Namespace} Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserName} {@Request}",
            _appSetting.App.Namespace,
            requestName,
            elapsedMilliseconds,
            userName,
            request);

        return response;
    }
}
