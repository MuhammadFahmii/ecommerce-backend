// ------------------------------------------------------------------------------------
// Changelog.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Domain.Entities;

/// <summary>
/// Changelog
/// </summary>
public class Changelog
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets method
    /// </summary>
    /// <value></value>
    public string? Method { get; set; }

    /// <summary>
    /// Gets or sets tableName
    /// </summary>
    /// <value></value>
    public string? TableName { get; set; }

    /// <summary>
    /// Gets or sets tableName
    /// </summary>
    /// <value></value>
    public string? KeyValues { get; set; }

    /// <summary>
    /// Gets or sets newValues
    /// </summary>
    /// <value></value>
    public string? NewValues { get; set; }

    /// <summary>
    /// Gets or sets oldValues
    /// </summary>
    /// <value></value>
    public string? OldValues { get; set; }

    /// <summary>
    /// Gets or sets changeDate
    /// </summary>
    /// <value></value>
    public long? ChangeDate { get; set; }
}