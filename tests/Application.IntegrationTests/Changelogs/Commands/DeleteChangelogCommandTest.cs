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
using netca.Application.IntegrationTests.Data;
using netca.Domain.Entities;
using netca.Infrastructure.Persistence;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.Changelogs.Commands;

using static Testing;

/// <summary>
/// DeleteChangelogCommandTest
/// </summary>
[TestFixtureSource(typeof(ChangelogDataTest), nameof(ChangelogDataTest.FixtureParams))]
public class DeleteChangelogCommandTest : TestBase
{
    private readonly long _changeDate;
    private readonly bool _shouldDelete;
    
    /// <summary>
    /// DeleteChangelogCommandTest
    /// </summary>
    /// <param name="changeDate"></param>
    /// <param name="shouldDelete"></param>
    public DeleteChangelogCommandTest(long changeDate, bool shouldDelete)
    {
        _changeDate = changeDate;
        _shouldDelete = shouldDelete;
    }
    
    /// <summary>
    /// ShouldDeleteChangelog
    /// </summary>
    [Test]
    public async Task ShouldDeleteChangelog()
    {
        using var scope = ScopeFactory?.CreateScope();
        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var query = new DeleteChangelogCommand();

        var id = Guid.NewGuid();
        await AddAsync(new Changelog
        {
            Id = id,
            ChangeDate = _changeDate
        });

        var test = await context?.Changelogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id))!;

        test.Should().NotBeNull();

        (await SendAsync(query)).Should().Be(Unit.Value);

        test = await context.Changelogs
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (_shouldDelete)
            test.Should().BeNull();
        else
            test.Should().NotBeNull();
    }
}