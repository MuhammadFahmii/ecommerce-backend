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
        /// Now
        /// </summary>
        /// <value></value>
        DateTime Now { get; }

        /// <summary>
        /// UtcNow
        /// </summary>
        /// <value></value>
        DateTime UtcNow { get; }
    }
}
