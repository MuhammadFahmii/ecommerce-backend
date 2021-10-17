// ------------------------------------------------------------------------------------
// IDomainEventService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using netca.Domain.Common;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IDomainEventService
    /// </summary>
    public interface IDomainEventService
    {
        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        Task Publish(DomainEvent domainEvent);
    }
}
