// ------------------------------------------------------------------------------------
// TodosVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using netca.Application.Common.Vms;

namespace netca.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// TodosVm
    /// </summary>
    public class TodosVm
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// PriorityLevels
        /// </summary>
        public IList<PriorityLevelVm> PriorityLevels { get; set; }
        
        /// <summary>
        /// Lists
        /// </summary>
        public IList<TodoListVm> Lists { get; set; }
    }
}
