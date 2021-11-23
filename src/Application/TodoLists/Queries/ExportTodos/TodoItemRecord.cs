// ------------------------------------------------------------------------------------
// TodoItemRecord.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Application.Common.Mappings;
using netca.Domain.Entities;

namespace netca.Application.TodoLists.Queries.ExportTodos
{
    /// <summary>
    /// TodoItemRecord
    /// </summary>
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        /// <summary>
        /// Gets or sets title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether done
        /// </summary>
        public bool Done { get; set; }
    }
}
