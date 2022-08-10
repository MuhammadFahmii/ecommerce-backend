// ------------------------------------------------------------------------------------
// TodoItemDeletedEvent.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Domain.Common;
using netca.Domain.Entities;

namespace netca.Domain.Events;

/// <summary>
/// TodoItemDeletedEvent
/// </summary>
public class TodoItemDeletedEvent : BaseEvent
{
    public TodoItemDeletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}