// ------------------------------------------------------------------------------------
// EventHubRedisDto.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace ecommerce.Application.Dtos;

/// <summary>
/// EventHubRedisDto
/// </summary>
public record EventHubRedisDto
{
    /// <summary>
    /// Gets or sets eventHubName
    /// </summary>
    /// <value></value>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets value
    /// </summary>
    /// <value></value>
    public string? Value { get; set; }
}