// ------------------------------------------------------------------------------------
// IRedisService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
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
        /// <param name="key"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sub"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<string> SaveAsync(string key, string sub, string value, CancellationToken cancellationToken);

        /// <summary>
        /// GetAllValueWithKeyAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<RedisDto>> GetAllValueWithKeyAsync(string key);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteAsync(string key);
    }
}
