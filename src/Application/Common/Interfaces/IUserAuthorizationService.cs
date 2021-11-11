// ------------------------------------------------------------------------------------
// ICurrentUserService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using netca.Application.Common.Models;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IUserAuthorizationService
    /// </summary>
    public interface IUserAuthorizationService
    {
        /// <summary>
        /// GetUserId
        /// </summary>
        /// <returns></returns>
        Guid GetUserId();

        /// <summary>
        /// GetUserName
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// GetCustomerCode
        /// </summary>
        /// <returns></returns>
        string GetCustomerCode();

        /// <summary>
        /// GetClientId
        /// </summary>
        /// <returns></returns>
        string GetClientId();

        /// <summary>
        /// GetAuthorizedUser
        /// </summary>
        /// <returns></returns>
        AuthorizedUser GetAuthorizedUser();

        /// <summary>
        /// GetUserAttributesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<string>>> GetUserAttributesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// GetUsersByAttributes
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="attributes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserClientIdInfo>> GetUsersByAttributesAsync(
            string serviceName, Dictionary<string, IList<string>> attributes, CancellationToken cancellationToken);

        /// <summary>
        /// GetNotifiedUsersAsync
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="attributes"></param>
        /// <param name="permission"></param>
        /// <param name="clientIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserClientIdInfo>> GetNotifiedUsersAsync(
            string serviceName, Dictionary<string, List<string>> attributes, string permission, IEnumerable<string> clientIds, CancellationToken cancellationToken);

        /// <summary>
        /// GetDevicesIdByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserDeviceInfo>> GetDevicesIdByUserIdAsync(Guid userId, string clientId, CancellationToken cancellationToken);

        /// <summary>
        /// GetEmailByUserIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserEmailInfo> GetEmailByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// GetGeneralParameterAsync
        /// </summary>
        /// <param name="generalParameterCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GeneralParameterInfo> GetGeneralParameterAsync(string generalParameterCode, CancellationToken cancellationToken);

        /// <summary>
        /// GetUserServices
        /// </summary>
        /// <param name="generalParameterCode"></param>
        /// <returns></returns>
        Task<List<string>> GetUserServicesAsync(string generalParameterCode);

        /// <summary>
        /// GetUserAttributesAndServicesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<string>>> GetUserAttributesAndServicesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// GetUserListAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserMangementUser>> GetUserListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// DeleteDeviceIdAsync
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task DeleteDeviceIdAsync(string deviceId);
    }
}