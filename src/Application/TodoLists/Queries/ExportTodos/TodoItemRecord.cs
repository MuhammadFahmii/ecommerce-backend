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
        /// Title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Done
        /// </summary>
        public bool Done { get; set; }
    }
}
