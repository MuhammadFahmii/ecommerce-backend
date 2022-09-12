// ------------------------------------------------------------------------------------
// TodoItemBriefVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Application.Common.Mappings;
using netca.Application.Common.Vms;
using netca.Domain.Entities;

namespace netca.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    /// <summary>
    /// TodoItemBriefVm
    /// </summary>
    public record TodoItemBriefVm : AuditTableVm, IMapFrom<TodoItem>
    {
        /// <summary>
        /// Gets or sets listId
        /// </summary>
        public Guid ListId { get; set; }

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
