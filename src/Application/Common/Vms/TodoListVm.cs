// ------------------------------------------------------------------------------------
// TodoListVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using netca.Application.Common.Mappings;
using netca.Domain.Entities;
using netca.Domain.ValueObjects;

namespace netca.Application.Common.Vms
{
    /// <summary>
    /// TodoListVm
    /// </summary>
    public class TodoListVm : AuditTableVm, IMapFrom<TodoList>
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Colour
        /// </summary>
        public Colour Colour { get; set; } = Colour.White;
        
        /// <summary>
        /// Items
        /// </summary>
        public IList<TodoItemVm> Items { get; set; } = new List<TodoItemVm>();
    }
}
