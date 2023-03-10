// ------------------------------------------------------------------------------------
// PriorityLevelVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Application.TodoLists.Queries.GetTodos
{
    /// <summary>
    /// PriorityLevelVm
    /// </summary>
    public record PriorityLevelVm
    {
        /// <summary>
        /// Gets or sets value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string? Name { get; set; }
    }
}
