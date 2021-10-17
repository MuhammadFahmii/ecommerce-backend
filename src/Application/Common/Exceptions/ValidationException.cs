// ------------------------------------------------------------------------------------
// ValidationException.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace netca.Application.Common.Exceptions
{
    /// <summary>
    /// ValidationException
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// ValidationException
        /// </summary>
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// ValidationException
        /// </summary>
        /// <param name="failures"></param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        /// <summary>
        /// Errors
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }
    }
}