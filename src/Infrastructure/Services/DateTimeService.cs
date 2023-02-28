// ------------------------------------------------------------------------------------
// DateTimeService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using ecommerce.Application.Common.Extensions;
using ecommerce.Application.Common.Interfaces;

namespace ecommerce.Infrastructure.Services;

/// <summary>
/// DateTimeService
/// </summary>
public class DateTimeService : IDateTime
{
    /// <summary>
    /// Gets now
    /// </summary>
    public long Now => DateExtensions.GetUnixLocalTimestamp();

    /// <summary>
    /// Gets utcNow
    /// </summary>
    public long UtcNow => DateExtensions.GetUnixUtcTimestamp();
}