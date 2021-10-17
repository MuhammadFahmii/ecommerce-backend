// ------------------------------------------------------------------------------------
// TodoItemCompletedEvent.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Domain.Common;
using netca.Domain.Entities;

namespace netca.Domain.Events
{
    /// <summary>
    /// TodoItemCompletedEvent
    /// </summary>
    public class TodoItemCompletedEvent : DomainEvent
    {
        /// <summary>
        /// TodoItemCompletedEvent
        /// </summary>
        /// <param name="item"></param>
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }
        
        /// <summary>
        /// TodoItem
        /// </summary>
        public TodoItem Item { get; }
    }
}
