// ------------------------------------------------------------------------------------
// DeleteChangelogCommandTest.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using netca.Application.Changelogs.Commands.DeleteChangelog;
using netca.Domain.Entities;
using netca.Infrastructure.Persistence;
using NUnit.Framework;
using static netca.Application.IntegrationTests.Testing;

namespace netca.Application.IntegrationTests.Changelogs.Commands;

/// <summary>
/// DeleteChangelogCommandTest
/// </summary>
public class DeleteChangelogCommandTest : TestBase
{
    /// <summary>
    /// ShouldDeleteChangelog
    /// </summary>
    /// <param name="changeDate"></param>
    /// <param name="shouldDelete"></param>
    [Test]
    [TestCase("2000-01-01", true)]
    [TestCase("2100-01-01", false)]
    public async Task ShouldDeleteChangelog(DateTime changeDate, bool shouldDelete)
    {
        using var scope = ScopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var query = new DeleteChangelogCommand();

        var id = Guid.NewGuid();
        await AddAsync(new Changelog
        {
            Id = id,
            ChangeDate = changeDate
        });

        var test = await context?.Changelogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id))!;

        test.Should().NotBeNull();

        (await SendAsync(query)).Should().Be(Unit.Value);

        test = await context.Changelogs
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (shouldDelete)
            test.Should().BeNull();
        else
            test.Should().NotBeNull();
    }
}