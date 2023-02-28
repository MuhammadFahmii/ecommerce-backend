// ------------------------------------------------------------------------------------
// TodosVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using ecommerce.Application.Common.Vms;

namespace ecommerce.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// TodosVm
    /// </summary>
    public record TodosVm
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
