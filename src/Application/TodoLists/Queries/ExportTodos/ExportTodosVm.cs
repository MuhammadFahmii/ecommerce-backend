// ------------------------------------------------------------------------------------
// ExportTodosVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Application.TodoLists.Queries.ExportTodos
{
    /// <summary>
    /// ExportTodosVm
    /// </summary>
    public class ExportTodosVm
    {
        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary>
        /// Content
        /// </summary>
        public byte[] Content { get; set; }
    }
}
