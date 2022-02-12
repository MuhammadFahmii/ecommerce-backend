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

namespace netca.Domain.Entities;

/// <summary>
/// TodoItem
/// </summary>
public class TodoItem : AuditTableEntity, IHasDomainEvent
{
    /// <summary>
    /// Gets or sets list
    /// </summary>
    public TodoList? List { get; set; }

    /// <summary>
    /// Gets or sets listId
    /// </summary>
    public Guid ListId { get; set; }

    /// <summary>
    /// Gets or sets title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets note
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Gets or sets priority
    /// </summary>
    public PriorityLevel Priority { get; set; }

    /// <summary>
    /// Gets or sets reminder
    /// </summary>
    public DateTime? Reminder { get; set; }

    private bool _done;

    /// <summary>
    /// Gets or sets a value indicating whether done
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
    /// Gets or sets domainEvents
    /// </summary>
    public List<DomainEvent> DomainEvents { get; set; } = new();
}