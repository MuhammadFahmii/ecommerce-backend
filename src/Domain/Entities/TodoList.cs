// ------------------------------------------------------------------------------------
// TodoList.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Domain.Entities;

/// <summary>
/// TodoList
/// </summary>
public record TodoList : BaseAuditableEntity
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
    /// Gets items
    /// </summary>
    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}