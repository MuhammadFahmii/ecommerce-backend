// ------------------------------------------------------------------------------------
// ExportTodosVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Application.TodoLists.Queries.ExportTodos
{
    /// <summary>
    /// ExportTodosVm
    /// </summary>
    public class ExportTodosVm
    {
        /// <summary>
        /// Gets or sets fileName
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets contentType
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets content
        /// </summary>
        public byte[]? Content { get; set; }
    }
}
