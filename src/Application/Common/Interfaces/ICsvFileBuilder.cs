// ------------------------------------------------------------------------------------
// ICsvFileBuilder.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Application.TodoLists.Queries.ExportTodos;

namespace netca.Application.Common.Interfaces;

/// <summary>
/// ICsvFileBuilder
/// </summary>
public interface ICsvFileBuilder
{
    /// <summary>
    /// BuildTodoItemsFile
    /// </summary>
    /// <param name="records"></param>
    /// <returns></returns>
    byte[]? BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}