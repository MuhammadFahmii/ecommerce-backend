// ------------------------------------------------------------------------------------
// RedisDto.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Application.Dtos;

/// <summary>
/// RedisDto
/// </summary>
public record RedisDto
{
    /// <summary>
    /// Gets or sets code
    /// </summary>
    /// <value></value>
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets desc
    /// </summary>
    /// <value></value>
    public string? Value { get; set; }
}