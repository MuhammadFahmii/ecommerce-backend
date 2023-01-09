// ------------------------------------------------------------------------------------
// ApiUserGroupCustomAttribute.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Api.Filters;

/// <summary>
/// ApiUserGroupCustomAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public abstract class ApiUserGroupCustomAttribute : Attribute
{
    /// <summary>
    /// Gets or sets group
    /// </summary>
    public string[]? Group { get; set; }
}