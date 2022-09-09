// ------------------------------------------------------------------------------------
// TodoListVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Application.Common.Mappings;
using netca.Domain.Entities;
using netca.Domain.ValueObjects;

namespace netca.Application.Common.Vms;

/// <summary>
/// TodoListVm
/// </summary>
public record TodoListVm : AuditTableVm, IMapFrom<TodoList>
{
    /// <summary>
    /// Gets or sets title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets colour
    /// </summary>
    public Colour Colour { get; set; } = Colour.White;

    /// <summary>
    /// Gets or sets items
    /// </summary>
    public IList<TodoItemVm> Items { get; set; } = new List<TodoItemVm>();
}