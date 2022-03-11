// ------------------------------------------------------------------------------------
// LoggingBehaviour.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Interfaces;

namespace netca.Application.Common.Behaviours;

/// <summary>
/// LoggingBehaviour
/// </summary>
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    
    /// <summary>
    /// LoggingBehaviour
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="currentUserService"></param>
    public LoggingBehaviour(ILogger<TRequest> logger, IUserAuthorizationService currentUserService)
    {
        _logger = logger;
        _userAuthorizationService = currentUserService;
    }
    
    /// <summary>
    /// Process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var user = _userAuthorizationService.GetAuthorizedUser();
        await Task.Delay(0, cancellationToken);
        _logger.LogDebug("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, user.UserId, user.UserName, request);
    }
}