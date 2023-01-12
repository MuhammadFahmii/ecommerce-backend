// ------------------------------------------------------------------------------------
// UserAuthorizationService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Extensions;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using netca.Application.Dtos;
using Newtonsoft.Json;

namespace netca.Infrastructure.Services;

/// <summary>
/// UserAuthorizationService
/// </summary>
public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly ClaimsPrincipal? _user;
    private readonly AppSetting _appSetting;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<UserAuthorizationService> _logger;
    private readonly bool _isAuthenticated;
    private readonly string? _permissionName;
    private readonly string _authorization;

    private static readonly HttpClient _httpClient = new(new HttpHandler(new HttpClientHandler())
    {
        UsingCircuitBreaker = true,
        UsingWaitRetry = true,
        RetryCount = 4,
        SleepDuration = 1000
    });

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthorizationService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSetting"></param>
    /// <param name="environment"></param>
    /// <param name="httpContextAccessor"></param>
    public UserAuthorizationService(
        ILogger<UserAuthorizationService> logger, AppSetting appSetting, IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _user = httpContextAccessor.HttpContext?.User;
        _appSetting = appSetting;
        _environment = environment;
        _authorization = httpContextAccessor?.HttpContext?.Request?.Headers?["Authorization"] ?? string.Empty;
        
        if (httpContextAccessor?.HttpContext?.Items.TryGetValue("CurrentPolicyName", out _) == true)
        {
            _permissionName = (string)httpContextAccessor.HttpContext?.Items["CurrentPolicyName"]!;
        }
        _isAuthenticated = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) != null;

        _httpClient.DefaultRequestHeaders.Add(
            _appSetting.AuthorizationServer.Header,
            _appSetting.AuthorizationServer.Secret
        );
    }

    private bool IsProd() => _environment.IsProduction();

    private bool IsTest() => _environment.IsEnvironment("Test");

    /// <summary>
    /// GetUserId
    /// </summary>
    /// <returns></returns>
    public Guid GetUserId()
    {
        if (!IsProd() && !_isAuthenticated)
            return Guid.NewGuid();

        var userId = _user?.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        return new Guid(userId ?? string.Empty);
    }

    /// <summary>
    /// GetUserName
    /// </summary>
    /// <returns></returns>
    public string? GetUserName()
    {
        return _user?.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
    }

    /// <summary>
    /// GetCustomerCode
    /// </summary>
    /// <returns></returns>
    public string? GetCustomerCode()
    {
        if (!IsProd() && !_isAuthenticated)
            return Constants.SystemCustomerName;

        return _user?.Claims.FirstOrDefault(i => i.Type == Constants.CustomerCode)?.Value;
    }

    /// <summary>
    /// GetClientId
    /// </summary>
    /// <returns></returns>
    public string? GetClientId()
    {
        if (!IsProd() && !_isAuthenticated)
            return Constants.SystemClientId;

        return _user?.Claims.FirstOrDefault(i => i.Type == Constants.ClientId)?.Value;
    }

    /// <summary>
    /// GetAuthorizedUser
    /// </summary>
    /// <returns></returns>
    public AuthorizedUser GetAuthorizedUser()
    {
        var result = MockData.GetAuthorizedUser();

        if (!IsProd() && !_isAuthenticated)
            return result;

        if (_isAuthenticated)
        {
            result = new AuthorizedUser()
            {
                UserId = GetUserId(),
                UserName = StringExtensions.Truncate(GetUserName(), 50),
                UserFullName = StringExtensions.Truncate(
                    $"{_user?.Claims.FirstOrDefault(i => i.Type == ClaimTypes.GivenName)?.Value} {_user?.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Surname)?.Value}",
                    50),
                CustomerCode = GetCustomerCode(),
                ClientId = GetClientId()
            };
        }

        return result;
    }

    /// <summary>
    /// GetUserAttributesAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, List<string>>?> GetUserAttributesAsync(CancellationToken cancellationToken)
    {
        Dictionary<string, List<string>>? result = null;
        if (!IsProd() && !_isAuthenticated)
            return MockData.GetUserAttribute();

        if (IsTest())
            return MockData.GetUserAttribute();

        if (!_isAuthenticated || _permissionName == null)
            return null;

        var userId = GetUserId();
        var clientId = GetClientId();
        var url = new Uri(_appSetting.AuthorizationServer.Gateway +
                          $"/api/user/attribute/{userId}/{clientId}/{_permissionName}");

        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return null;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result;
    }

    /// <summary>
    /// GetUsersByAttributes
    /// </summary>
    /// <param name="serviceName"></param>
    /// <param name="attributes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<UserClientIdInfo>> GetUsersByAttributesAsync(
        string serviceName, Dictionary<string, IList<string>> attributes, CancellationToken cancellationToken)
    {
        if (IsTest())
            return MockData.GetUserByAttribute();

        var result = new List<UserClientIdInfo>();

        if (!_isAuthenticated)
            return result;

        var jsonString = JsonConvert.SerializeObject(attributes);

        var url = new Uri(_appSetting.AuthorizationServer.Gateway + $"/api/user/attributes/{serviceName}");

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
            cancellationToken
        );

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<UserClientIdInfo>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetNotifiedUsers
    /// </summary>
    /// <param name="serviceName"></param>
    /// <param name="attributes"></param>
    /// <param name="permission"></param>
    /// <param name="clientIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<UserClientIdInfo>> GetNotifiedUsersAsync(
        string serviceName, Dictionary<string, List<string>> attributes, string permission,
        IEnumerable<string> clientIds, CancellationToken cancellationToken)
    {
        var result = new List<UserClientIdInfo>();

        if (!_isAuthenticated)
            return result;

        var jsonString = JsonConvert.SerializeObject(attributes);
        var url = $"/api/user/attributes/notification/{serviceName}?notifPermission={permission}";
        var enumerable = clientIds.ToList();

        if (enumerable.Any())
            url = enumerable.Aggregate(url, (current, clientId) => current + $"&clientId={clientId}");

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
            cancellationToken
        );

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<UserClientIdInfo>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetDevicesIdByUserId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="clientId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<UserDeviceInfo>> GetDevicesIdByUserIdAsync(
        Guid userId, string clientId, CancellationToken cancellationToken)
    {
        var result = new List<UserDeviceInfo>();

        if (!_isAuthenticated)
            return result;

        var url = _appSetting.AuthorizationServer.Gateway + $"/api/user/device/{userId}/{clientId}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<UserDeviceInfo>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetEmailByUserId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UserEmailInfo> GetEmailByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = new UserEmailInfo();

        if (!_isAuthenticated)
            return result;

        var url = _appSetting.AuthorizationServer.Gateway + $"/api/user/email/{userId}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<UserEmailInfo>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetGeneralParameter
    /// </summary>
    /// <param name="generalParameterCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GeneralParameterInfo> GetGeneralParameterAsync(
        string generalParameterCode, CancellationToken cancellationToken)
    {
        var result = new GeneralParameterInfo();

        if (!_isAuthenticated)
            return result;

        var url = new Uri(
            _appSetting.AuthorizationServer.Gateway + "api/generalparameter/" + generalParameterCode);
        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<GeneralParameterInfo>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetUserServicesAsync
    /// </summary>
    /// <param name="generalParameterCode"></param>
    /// <returns></returns>
    public Task<List<string>> GetUserServicesAsync(string generalParameterCode)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// GetUserAttributesAndServices
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, List<string>>> GetUserAttributesAndServicesAsync(
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, List<string>>();

        if (!_isAuthenticated || _permissionName == null)
            return result;

        var userId = GetUserId();
        var clientId = GetClientId();
        var url = _appSetting.AuthorizationServer.Gateway +
                  $"/api/user/attribute/service/{userId}/{clientId}/{_permissionName}?getService=true";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// GetUserListAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<UserManagementUser>> GetUserListAsync(CancellationToken cancellationToken)
    {
        var result = new List<UserManagementUser>();

        if (IsTest())
            return MockData.GetListMechanics();

        if (!_isAuthenticated)
            return result;

        var clientId = GetClientId();
        var url = _appSetting.AuthorizationServer.Gateway + $"/api/user/clientid/{clientId}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogDebug("Response: {Code}", response.IsSuccessStatusCode);
        _logger.LogDebug("{M}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<UserManagementUser>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed HTTP request status {Status} {Response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// DeleteDeviceIdAsync
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public async Task DeleteDeviceIdAsync(string deviceId)
    {
        if (!_isAuthenticated)
            return;

        var url = _appSetting.AuthorizationServer.Gateway + $"/api/deviceid/{deviceId}";
        await _httpClient.DeleteAsync(url);
    }

    /// <summary>
    /// GetPermissionListAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<PermissionUms>> GetPermissionListAsync(
        Guid applicationId, CancellationToken cancellationToken)
    {
        var result = new List<PermissionUms>();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var url = $"/api/permission/all?applicationId={applicationId}";
        var response = await _httpClient.GetAsync(new Uri(_httpClient.BaseAddress + url), cancellationToken);

        _logger.LogDebug("Response: \n {response}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<PermissionUms>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to HTTP request with status {statusCode}: {response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// CreatePermissionsAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="permissions"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<ResponsePermissionUmsDto>> CreatePermissionsAsync(
        Guid applicationId,
        List<PermissionDto> permissions,
        CancellationToken cancellationToken)
    {
        var result = new List<ResponsePermissionUmsDto>();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var url = new Uri(_httpClient.BaseAddress + $"api/permission?applicationId={applicationId}");

        foreach (var permission in permissions)
        {
            var jsonString = JsonConvert.SerializeObject(permission);
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
                cancellationToken);

            _logger.LogDebug("Response: \n {response}", response.ToString());

            result.Add(new ResponsePermissionUmsDto
            {
                Name = permission.Path,
                Status = response?.ReasonPhrase!
            });
        }

        return result;
    }

    /// <summary>
    /// GetGroupListAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<GroupUms>> GetGroupListAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var result = new List<GroupUms>();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var url = $"/api/group/all/{applicationId}?status=1";
        var response = await _httpClient.GetAsync(new Uri(_httpClient.BaseAddress + url), cancellationToken);

        _logger.LogDebug("Response: \n {response}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<GroupUms>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to HTTP request with status {statusCode}: {response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// CreateGroupAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="groupCode"></param>
    /// <param name="groupId"></param>
    /// <param name="permissionIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ResponseGroupRoleUmsDto> CreateGroupAsync(
        Guid applicationId,
        string groupCode,
        Guid? groupId,
        List<Guid> permissionIds,
        CancellationToken cancellationToken)
    {
        var result = new ResponseGroupRoleUmsDto();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var data = new GroupUms
        {
            GroupId = groupId,
            ApplicationId = applicationId,
            GroupCode = groupCode,
            PermissionIds = permissionIds,
            Status = true
        };

        var jsonString = JsonConvert.SerializeObject(data);

        if (groupId != null)
        {
            var url = new Uri(_httpClient.BaseAddress + $"/api/group/{groupId}");

            var response = await _httpClient.PutAsync(
                url,
                new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
                cancellationToken);

            _logger.LogDebug("Response: \n {response}", response.ToString());

            result.Request = "PUT";
            result.Status = response?.ReasonPhrase!;
        }
        else
        {
            var url = new Uri(_httpClient.BaseAddress + "/api/group");

            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
                cancellationToken);

            _logger.LogDebug("Response: \n {response}", response.ToString());

            result.Request = "POST";
            result.Status = response?.ReasonPhrase!;
        }

        return result;
    }

    /// <summary>
    /// GetRoleListAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<RoleUms>> GetRoleListAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var result = new List<RoleUms>();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var url = $"/api/role/all/{applicationId}";
        var response = await _httpClient.GetAsync(new Uri(_httpClient.BaseAddress + url), cancellationToken);

        _logger.LogDebug("Response: \n {response}", response.ToString());

        if (!response.IsSuccessStatusCode)
            return result;

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            result = JsonConvert.DeserializeObject<List<RoleUms>>(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to HTTP request with status {statusCode}: {response}", response.StatusCode, responseString);
        }

        return result!;
    }

    /// <summary>
    /// CreateRoleAsync
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="roleCode"></param>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ResponseGroupRoleUmsDto> CreateRoleAsync(
        Guid applicationId,
        string roleCode,
        Guid? roleId,
        List<Guid> groupIds,
        CancellationToken cancellationToken)
    {
        var result = new ResponseGroupRoleUmsDto();

        if (string.IsNullOrEmpty(_authorization))
            throw new BadRequestException("Token cannot be empty");

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Add("Authorization", _authorization);

        var data = new RoleUms
        {
            RoleId = roleId,
            ApplicationId = applicationId,
            RoleCode = roleCode,
            RoleLevel = 6,
            RoleType = "User",
            PublicInformation = false,
            GroupIds = groupIds,
            Status = true,
            UpdatedDate = DateTime.UtcNow
        };

        var jsonString = JsonConvert.SerializeObject(data);

        if (roleId != null)
        {
            var url = new Uri(_httpClient.BaseAddress + $"/api/role/{roleId}");

            var response = await _httpClient.PutAsync(
                url,
                new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
                cancellationToken);

            _logger.LogDebug("Response: \n {response}", response.ToString());

            result.Request = "PUT";
            result.Status = response?.ReasonPhrase!;
        }
        else
        {
            var url = new Uri(_httpClient.BaseAddress + $"/api/role");

            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonString, Encoding.UTF8, Constants.HeaderJson),
                cancellationToken);

            _logger.LogDebug("Response: \n {response}", response.ToString());

            result.Request = "POST";
            result.Status = response?.ReasonPhrase!;
        }

        return result;
    }
}