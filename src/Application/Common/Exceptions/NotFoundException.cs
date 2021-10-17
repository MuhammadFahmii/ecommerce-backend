// ------------------------------------------------------------------------------------
// NotFoundException.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.Common.Exceptions
{
    /// <summary>
    /// NotFoundException
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// NotFoundException
        /// </summary>
        public NotFoundException()
            : base()
        {
        }
        
        /// <summary>
        /// NotFoundException
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string message)
            : base(message)
        {
        }
        
        /// <summary>
        /// NotFoundException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        
        /// <summary>
        /// NotFoundException
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
