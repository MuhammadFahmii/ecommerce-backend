// ------------------------------------------------------------------------------------
// TestBase.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using NUnit.Framework;

namespace netca.Application.IntegrationTests;
using static Testing;

/// <summary>
/// TestBase
/// </summary>
public class TestBase
{
    /// <summary>
    /// TestSetUp
    /// </summary>
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}