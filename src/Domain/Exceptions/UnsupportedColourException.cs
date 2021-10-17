// ------------------------------------------------------------------------------------
// UnsupportedColourException.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Domain.Exceptions
{
    /// <summary>
    /// UnsupportedColourException
    /// </summary>
    public class UnsupportedColourException : Exception
    {
        /// <summary>
        /// UnsupportedColourException
        /// </summary>
        /// <param name="code"></param>
        public UnsupportedColourException(string code)
            : base($"Colour \"{code}\" is unsupported.")
        {
        }
    }
}
