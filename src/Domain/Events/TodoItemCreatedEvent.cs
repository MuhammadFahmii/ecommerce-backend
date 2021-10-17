// ------------------------------------------------------------------------------------
// TodoItemCreatedEvent.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Domain.Common;
using netca.Domain.Entities;

namespace netca.Domain.Events
{
    /// <summary>
    /// TodoItemCreatedEvent
    /// </summary>
    public class TodoItemCreatedEvent : DomainEvent
    {
        /// <summary>
        /// TodoItemCreatedEvent
        /// </summary>
        /// <param name="item"></param>
        public TodoItemCreatedEvent(TodoItem item)
        {
            Item = item;
        }
        
        /// <summary>
        /// Item
        /// </summary>
        public TodoItem Item { get; }
    }
}
