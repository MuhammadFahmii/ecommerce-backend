// ------------------------------------------------------------------------------------
// TodoItemDataTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using netca.Domain.Enums;
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
    
    /// <summary>
    /// ShouldRequireMinimumFieldsUpdateDetail
    /// </summary>
    public static IEnumerable<TestCaseData> ShouldRequireMinimumFieldsUpdateDetail
    {
        get
        {
            yield return new TestCaseData(Guid.NewGuid(), null, PriorityLevel.High);
            yield return new TestCaseData(null, Guid.NewGuid(), PriorityLevel.High);
            yield return new TestCaseData(Guid.NewGuid(), Guid.NewGuid(), PriorityLevel.High);
            yield return new TestCaseData(Guid.NewGuid(), Guid.NewGuid(), 5);
            
        }
    }
    
    /// <summary>
    /// UpdateDetail
    /// </summary>
    public static IEnumerable<TestCaseData> UpdateDetail
    {
        get
        {
            yield return new TestCaseData(Guid.NewGuid(), Guid.NewGuid(), "Shopping");
        }
    }
    /// <summary>
    /// Delete
    /// </summary>
    public static IEnumerable<TestCaseData> Delete
    {
        get
        {
            yield return new TestCaseData(null, false);
            yield return new TestCaseData(Guid.NewGuid(), true);
        }
    }
}