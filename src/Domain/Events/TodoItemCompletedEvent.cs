// ------------------------------------------------------------------------------------
// TodoItemCompletedEvent.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Domain.Events;

/// <summary>
/// TodoItemCompletedEvent
/// </summary>
public class TodoItemCompletedEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItemCompletedEvent"/> class.
    /// TodoItemCompletedEvent
    /// </summary>
    /// <param name="item"></param>
    public TodoItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    /// <summary>
    /// Gets todoItem
    /// </summary>
    public TodoItem Item { get; }
}