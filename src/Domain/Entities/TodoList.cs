// ------------------------------------------------------------------------------------
// TodoList.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using netca.Domain.Common;
using netca.Domain.ValueObjects;

namespace netca.Domain.Entities
{
    /// <summary>
    /// TodoList
    /// </summary>
    public class TodoList : AuditTableEntity
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
        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
