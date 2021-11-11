// ------------------------------------------------------------------------------------
// IEHProducerService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using netca.Application.Common.Models;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IEHProducerService
    /// </summary>
    public interface IEHProducerService
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        Task<bool> SendAsync(AzureEventHub az, string topic, string message, CancellationToken cancellationToken);
    }
}
