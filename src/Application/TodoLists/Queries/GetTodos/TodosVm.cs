// ------------------------------------------------------------------------------------
// TodosVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using JsonApiSerializer.JsonApi;
using netca.Application.Common.Vms;

namespace netca.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// TodosVm
    /// </summary>
    public class TodosVm
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets priorityLevels
        /// </summary>
        public IList<PriorityLevelVm>? PriorityLevels { get; set; }

        /// <summary>
        /// Gets or sets lists
        /// </summary>
        public IList<TodoListVm>? Lists { get; set; }
    }
}
