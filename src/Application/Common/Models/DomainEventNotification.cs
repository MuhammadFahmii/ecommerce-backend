// ------------------------------------------------------------------------------------
// DomainEventNotification.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using MediatR;
using netca.Domain.Common;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// DomainEventNotification
    /// </summary>
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        /// <summary>
        /// DomainEventNotification
        /// </summary>
        /// <param name="domainEvent"></param>
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
        
        /// <summary>
        /// DomainEvent
        /// </summary>
        public TDomainEvent DomainEvent { get; }
    }
}
