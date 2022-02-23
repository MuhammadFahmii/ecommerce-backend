// ------------------------------------------------------------------------------------
// UserAuthorizationHandlerService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Models;
using Constants = netca.Application.Common.Models.Constants;

namespace netca.Infrastructure.Services;

/// <summary>
/// UserAuthorizationHandlerService
/// </summary>
public class UserAuthorizationHandlerService : AuthorizationHandler<Permission>
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserAuthorizationHandlerService> _logger;
    private readonly AppSetting _appSetting;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthorizationHandlerService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <summary>
    /// UserAuthorizationHandler
    /// </summary>
    /// <param name="appSetting"></param>
    public UserAuthorizationHandlerService(ILogger<UserAuthorizationHandlerService> logger, AppSetting appSetting)
    {
        _logger = logger;
        _appSetting = appSetting;
        _httpClient = new HttpClient(new HttpHandler(new HttpClientHandler()));
        _httpClient.DefaultRequestHeaders.Add(_appSetting.AuthorizationServer.Header,
            _appSetting.AuthorizationServer.Secret);
    }

    /// <summary>
    /// HandleRequirementAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Permission requirement)
    {
        var user = context.User;
        var userId = user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        var clientId = user.Claims.FirstOrDefault(i => i.Type == Constants.ClientId)?.Value;

        var url = new Uri(_appSetting.AuthorizationServer.Gateway +
                          $"/api/authorize/{userId}/{clientId}/{requirement.PermissionName}");
        var response = await _httpClient.GetAsync(url);
        _logger.LogDebug("Response:");
        _logger.LogDebug("{M}", response.ToString());
        if (response.IsSuccessStatusCode)
        {
            context.Succeed(requirement);
        }
    }
}

/// <summary>
/// Permission
/// </summary>
public class Permission : IAuthorizationRequirement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Permission"/> class.
    /// Permission
    /// </summary>
    /// <param name="permissionName"></param>
    public Permission(string permissionName)
    {
        PermissionName = permissionName;
    }

    /// <summary>
    /// Gets permissionName
    /// </summary>
    /// <value></value>
    public string PermissionName { get; }
}