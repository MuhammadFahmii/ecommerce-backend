// ------------------------------------------------------------------------------------
// CsvFileBuilder.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using ecommerce.Application.Common.Interfaces;
using ecommerce.Application.TodoLists.Queries.ExportTodos;
using ecommerce.Infrastructure.Files.Maps;

namespace ecommerce.Infrastructure.Files;

/// <summary>
/// CsvFileBuilder
/// </summary>
public class CsvFileBuilder : ICsvFileBuilder
{
    /// <summary>
    /// BuildTodoItemsFile
    /// </summary>
    /// <param name="records"></param>
    /// <returns></returns>
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}