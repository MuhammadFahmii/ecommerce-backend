// ------------------------------------------------------------------------------------
// DateTimeService.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using netca.Application.Common.Interfaces;

namespace netca.Infrastructure.Services;

/// <summary>
/// DateTimeService
/// </summary>
public class DateTimeService : IDateTime
{
    /// <summary>
    /// Gets now
    /// </summary>
    public DateTime Now => DateTime.Now;

    /// <summary>
    /// Gets utcNow
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;
}