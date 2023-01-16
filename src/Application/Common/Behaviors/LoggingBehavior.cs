// ------------------------------------------------------------------------------------
// LoggingBehavior.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;

namespace netca.Application.Common.Behaviors;

/// <summary>
/// LoggingBehavior
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="currentUserService"></param>
    /// <param name="appSetting"></param>
    public LoggingBehavior(
        ILogger<TRequest> logger,
        IUserAuthorizationService currentUserService,
        AppSetting appSetting)
    {
        _logger = logger;
        _userAuthorizationService = currentUserService;
        _appSetting = appSetting;
    }

    /// <summary>
    /// Process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var user = _userAuthorizationService.GetAuthorizedUser();
        await Task.Delay(0, cancellationToken);
        _logger.LogDebug(
            "{Namespace} Request: {Name} {@UserId} {@UserName} {@Request}",
            _appSetting.App.Namespace,
            requestName,
            user.UserId,
            user.UserName,
            request);
    }
}