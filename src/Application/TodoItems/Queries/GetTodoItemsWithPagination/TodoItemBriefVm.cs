// ------------------------------------------------------------------------------------
// TodoItemBriefVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using netca.Application.Common.Mappings;
using netca.Application.Common.Vms;
using netca.Domain.Entities;

namespace netca.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    /// <summary>
    /// TodoItemBriefVm
    /// </summary>
    public class TodoItemBriefVm : AuditTableVm, IMapFrom<TodoItem>
    {
        
        /// <summary>
        /// ListId
        /// </summary>
        public Guid ListId { get; set; }
        
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
