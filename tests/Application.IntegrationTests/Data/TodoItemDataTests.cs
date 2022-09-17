// ------------------------------------------------------------------------------------
// TodoItemDataTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.Data;

/// <summary>
/// TodoItemDataTests
/// </summary>
public class TodoItemDataTests
{
    /// <summary>
    /// ShouldRequireMinimumFields
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldRequireMinimumFields
    {
        get
        {
            yield return new TestCaseData(Guid.NewGuid(), Guid.NewGuid(), "Title");
            yield return new TestCaseData(Guid.NewGuid(), null, "Title");
        }
    }
    
    /// <summary>
    /// ShouldCreated
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldCreated
    {
        get
        {
            yield return new TestCaseData(Guid.NewGuid(), Guid.NewGuid(), "Shopping");
        }
    }
}