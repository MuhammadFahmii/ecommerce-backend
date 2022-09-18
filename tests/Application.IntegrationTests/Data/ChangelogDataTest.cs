// ------------------------------------------------------------------------------------
// ChangelogDataTest.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Collections.Generic;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.Data;

/// <summary>
/// ChangelogDataTest
/// </summary>
public class ChangelogDataTest
{
    /// <summary>
    /// FixtureParams
    /// </summary>
    public static IEnumerable<TestFixtureData> FixtureParams
    {
        get
        {
            yield return new TestFixtureData(946659600000, true);
            yield return new TestFixtureData(4102419600000, false);
        }
    } 
}