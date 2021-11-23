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
using netca.Application.Common.Extensions;
using netca.Application.Common.Interfaces;
using netca.Application.Common.Models;
using Newtonsoft.Json;

namespace netca.Infrastructure.Services
{
    /// <summary>
    /// UserAuthorizationService
    /// </summary>
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly ClaimsPrincipal? _user;
        private readonly AppSetting _appSetting;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserAuthorizationService> _logger;
        private readonly HttpClient _httpClient;
        private readonly bool _isAuthenticated;
        private readonly string? _permissionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthorizationService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appSetting"></param>
        /// <param name="environment"></param>
        /// <param name="httpContextAccessor"></param>
        public UserAuthorizationService(
            ILogger<UserAuthorizationService> logger, AppSetting appSetting, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = new HttpClient(new HttpHandler(new HttpClientHandler()));
            _user = httpContextAccessor.HttpContext?.User;
            _appSetting = appSetting;
            _environment = environment;
            _permissionName = (string)httpContextAccessor?.HttpContext?.Items["CurrentPolicyName"]!;

            if (httpContextAccessor != null)
                _isAuthenticated = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) != null;

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

            var userId = _user!.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            return new Guid(userId ?? string.Empty);
        }

        /// <summary>
        /// GetUserName
        /// </summary>
        /// <returns></returns>
        public string? GetUserName()
        {
            if (!IsProd() && !_isAuthenticated)
                return Constants.SystemEmail;

            return _user!.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// GetCustomerCode
        /// </summary>
        /// <returns></returns>
        public string? GetCustomerCode()
        {
            if (!IsProd() && !_isAuthenticated)
                return Constants.SystemCustomerName;

            return _user!.Claims.FirstOrDefault(i => i.Type == Constants.CustomerCode)?.Value;
        }

        /// <summary>
        /// GetClientId
        /// </summary>
        /// <returns></returns>
        public string? GetClientId()
        {
            if (!IsProd() && !_isAuthenticated)
                return Constants.SystemClientId;

            return _user!.Claims.FirstOrDefault(i => i.Type == Constants.ClientId)?.Value;
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
                        $"{_user!.Claims.FirstOrDefault(i => i.Type == ClaimTypes.GivenName)?.Value} {_user.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Surname)?.Value}",
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

            if (!_isAuthenticated)
                return null;

            var userId = GetUserId();
            var clientId = GetClientId();
            var url = new Uri(_appSetting.AuthorizationServer.Gateway +
                $"/api/user/attribute/{userId}/{clientId}/{_permissionName}");

            var response = await _httpClient.GetAsync(url, cancellationToken);

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return null;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<List<UserClientIdInfo>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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
            string serviceName, Dictionary<string, List<string>> attributes, string permission, IEnumerable<string> clientIds, CancellationToken cancellationToken)
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<List<UserClientIdInfo>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<List<UserDeviceInfo>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<UserEmailInfo>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<GeneralParameterInfo>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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
        public async Task<Dictionary<string, List<string>>> GetUserAttributesAndServicesAsync(CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, List<string>>();

            if (!_isAuthenticated)
                return result;

            var userId = GetUserId();
            var clientId = GetClientId();
            var url = _appSetting.AuthorizationServer.Gateway +
                $"/api/user/attribute/service/{userId}/{clientId}/{_permissionName}?getService=true";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed HTTP request status {response.StatusCode}: {responseString}");
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

            _logger.LogDebug("Response:");
            _logger.LogDebug(response.ToString());

            if (!response.IsSuccessStatusCode)
                return result;

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                result = JsonConvert.DeserializeObject<List<UserManagementUser>>(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed HTTP request status {response.StatusCode}: {responseString} \n {e.Message}");
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
    }
}