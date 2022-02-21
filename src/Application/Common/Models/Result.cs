// ------------------------------------------------------------------------------------
// Result.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace netca.Application.Common.Models;

/// <summary>
/// Result
/// </summary>
public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Gets or sets a value indicating whether succeeded
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets errors
    /// </summary>
    public string[] Errors { get; set; }

    /// <summary>
    /// Success
    /// </summary>
    /// <returns></returns>
    public static Result Success()
    {
        return new Result(true, System.Array.Empty<string>());
    }

    /// <summary>
    /// Failure
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}