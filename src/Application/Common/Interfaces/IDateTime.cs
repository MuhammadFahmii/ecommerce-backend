// ------------------------------------------------------------------------------------
// IDateTime.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IDateTime
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets now
        /// </summary>
        /// <value></value>
        DateTime Now { get; }

        /// <summary>
        /// Gets utcNow
        /// </summary>
        /// <value></value>
        DateTime UtcNow { get; }
    }
}