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
        /// Gets or sets domainEvents
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; }
    }

    /// <summary>
    /// DomainEvent
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> class.
        /// </summary>
        protected DomainEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets or sets a value indicating whether isPublished
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets dateOccurred
        /// </summary>
        public DateTimeOffset DateOccurred { get; protected set; }
    }
}