// ------------------------------------------------------------------------------------
// TodoItemCreatedEvent.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Domain.Events;

/// <summary>
/// TodoItemCreatedEvent
/// </summary>
public class TodoItemCreatedEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItemCreatedEvent"/> class.
    /// TodoItemCreatedEvent
    /// </summary>
    /// <param name="item"></param>
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    /// <summary>
    /// Gets item
    /// </summary>
    public TodoItem Item { get; }
}