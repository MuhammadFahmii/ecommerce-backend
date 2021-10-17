// ------------------------------------------------------------------------------------
// ThrowException.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.Common.Exceptions
{
    /// <summary>
    /// ThrowException
    /// </summary>
    public class ThrowException : Exception
    {
        /// <summary>
        /// ThrowException
        /// </summary>
        /// <param name="message"></param>
        public ThrowException(string message)
            : base(message)
        {
        }
    }
}
