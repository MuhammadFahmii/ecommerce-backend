// ------------------------------------------------------------------------------------
// AppLoggingExtension.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace ecommerce.Application.Common.Extensions;

/// <summary>
/// Shared logger
/// </summary>
public static class AppLoggingExtensions
{
    /// <summary>
    /// Gets or sets loggerFactory
    /// </summary>
    /// <value></value>
    public static ILoggerFactory? LoggerFactory { get; set; }

    /// <summary>
    /// CreateLogger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ILogger<T>? CreateLogger<T>() => LoggerFactory?.CreateLogger<T>();

    /// <summary>
    /// CreateLogger
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public static ILogger? CreateLogger(string categoryName) => LoggerFactory?.CreateLogger(categoryName);
}