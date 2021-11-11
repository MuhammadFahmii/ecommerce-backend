// ------------------------------------------------------------------------------------
// TodoItemRecordMap.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Globalization;
using CsvHelper.Configuration;
using netca.Application.TodoLists.Queries.ExportTodos;

namespace netca.Infrastructure.Files.Maps
{
    /// <summary>
    /// TodoItemRecordMap
    /// </summary>
    public class TodoItemRecordMap : ClassMap<TodoItemRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TodoItemRecordMap"/> class.
        /// </summary>
        public TodoItemRecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
        }
    }
}
