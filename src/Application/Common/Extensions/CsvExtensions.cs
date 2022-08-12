﻿// ------------------------------------------------------------------------------------
// CsvExtensions.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;

namespace netca.Application.Common.Extensions;

/// <summary>
/// Csv Extension
/// </summary>
public static class CsvExtensions
{
    /// <summary>
    /// Write Csv
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static byte[] ToCsv<T>(this IEnumerable<T> collection)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        using var stream = new MemoryStream();
        stream.Position = 0;

        using var writeFile = new StreamWriter(stream);
        writeFile.AutoFlush = true;

        using var csv = new CsvWriter(writeFile, config);
        csv.WriteRecords(collection);

        return stream.ToArray();
    }

    /// <summary>
    /// Read Csv
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    public static T[] FromCsv<T>(this IFormFile file)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();

        return csv.GetRecords<T>().ToArray();
    }
}