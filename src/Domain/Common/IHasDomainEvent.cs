// ------------------------------------------------------------------------------------
// IHasDomainEvent.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace netca.Domain.Common
{
    /// <summary>
    /// IHasDomainEvent
    /// </summary>
    public interface IHasDomainEvent
    {
        /// <summary>
        /// DomainEvents
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; }
    }
    
    /// <summary>
    /// DomainEvent
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// DomainEvent
        /// </summary>
        protected DomainEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }
        
        /// <summary>
        /// IsPublished
        /// </summary>
        public bool IsPublished { get; set; }
        
        /// <summary>
        /// DateOccurred
        /// </summary>
        public DateTimeOffset DateOccurred { get; protected set; }
    }
}
