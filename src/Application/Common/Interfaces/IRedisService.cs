// ------------------------------------------------------------------------------------
// IRedisService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using netca.Application.Dtos;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IRedisService
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sub"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<string> SaveAsync(string key, string sub, string value);

        /// <summary>
        /// GetAllValueWithKeyAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IEnumerable<RedisDto>> GetAllValueWithKeyAsync(string key);

        /// <summary>
        /// ListLeftPushAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> ListLeftPushAsync(string key, string value);

        /// <summary>
        /// ListLeftPopAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> ListLeftPopAsync(string key);

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteAsync(string key);
    }
}
