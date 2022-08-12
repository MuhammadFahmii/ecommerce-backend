// ------------------------------------------------------------------------------------
// BaseService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Application.Common.Extensions;

namespace netca.Infrastructure.Services;

/// <summary>
/// BaseService
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Generates a key for a Redis Entry  , follows the Redis Name Convention of inserting a column : to identify values
    /// </summary>
    /// <param name="key">Redis identifier key</param>
    /// <param name="sub">Redis sub key</param>
    /// <returns>concatenates the key with the name of the type</returns>
    protected string GenerateKey(string key, string sub)
    {
        return string.Concat(DateExtensions.GetUnixUtcTimestamp(), ":", sub.ToLower(), ":", key.ToLower());
    }
}