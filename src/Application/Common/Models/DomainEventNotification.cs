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
    /// <typeparam name="TDomainEvent"></typeparam>
    public class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventNotification{TDomainEvent}"/> class.
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
