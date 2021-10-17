// ------------------------------------------------------------------------------------
// TodoItem.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using netca.Domain.Common;
using netca.Domain.Enums;
using netca.Domain.Events;

namespace netca.Domain.Entities
{
    /// <summary>
    /// TodoItem
    /// </summary>
    public class TodoItem : AuditTableEntity, IHasDomainEvent
    {
        /// <summary>
        /// List
        /// </summary>
        public TodoList List { get; set; }
        
        /// <summary>
        /// ListId
        /// </summary>
        public Guid ListId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// Priority
        /// </summary>
        public PriorityLevel Priority { get; set; }
        
        /// <summary>
        /// Reminder
        /// </summary>
        public DateTime? Reminder { get; set; }
    
        private bool _done;
        
        /// <summary>
        /// Done
        /// </summary>
        public bool Done
        {
            get => _done;
            set
            {
                if (value && !_done)
                {
                    DomainEvents.Add(new TodoItemCompletedEvent(this));
                }

                _done = value;
            }
        }
        
        /// <summary>
        /// DomainEvents
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
