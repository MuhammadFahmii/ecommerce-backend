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
        /// Get
        /// </summary>
        Task<string> GetAsync(string key);

        /// <summary>
        /// SaveAsync
        /// </summary>
        Task<string> SaveAsync(string key, string sub, string value);

        /// <summary>
        /// GetAllValueWithKeyAsync
        /// </summary>
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
        /// Delete
        /// </summary>
        Task DeleteAsync(string key); 
    }
}
