// ------------------------------------------------------------------------------------
// TodoListDataTest.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.Data;

/// <summary>
/// TodoListDataTest
/// </summary>
public class TodoListDataTest
{
    /// <summary>
    /// ShouldRequireMinimumFields
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldRequireMinimumFields
    {
        get
        {
            yield return new TestCaseData(null, "Title");
            yield return new TestCaseData(Guid.NewGuid(), null);
        }
    }
    
    /// <summary>
    /// ShouldRequireValidTodoListId
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldRequireValidTodoListId
    {
        get
        {
            yield return new TestCaseData(null, "Title", false);
            yield return new TestCaseData(Guid.NewGuid(), null, false);
            yield return new TestCaseData(Guid.NewGuid(), "Title", true);
        }
    }
    
    /// <summary>
    /// ShouldCreated
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldCreated
    {
        get
        {
            yield return new TestCaseData(Guid.NewGuid(), "Shopping");
        }
    }
}