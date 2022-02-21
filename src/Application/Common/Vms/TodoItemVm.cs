// ------------------------------------------------------------------------------------
// TodoItemVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using AutoMapper;
using netca.Application.Common.Mappings;
using netca.Domain.Entities;
using netca.Domain.Enums;

namespace netca.Application.Common.Vms;

/// <summary>
/// TodoItemVm
/// </summary>
public class TodoItemVm : AuditTableVm, IMapFrom<TodoItem>
{
    /// <summary>
    /// Gets or sets list
    /// </summary>
    public TodoListVm? List { get; set; }

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

    /// <summary>
    /// Gets or sets a value indicating whether done
    /// </summary>
    public bool Done { get; set; }

    /// <summary>
    /// Mapping
    /// </summary>
    /// <param name="profile"></param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<TodoItem, TodoItemVm>()
            .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
    }
}