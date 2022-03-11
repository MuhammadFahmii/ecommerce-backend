// ------------------------------------------------------------------------------------
// UpdateTodoListTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Application.TodoLists.Commands.UpdateTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

/// <summary>
/// UpdateTodoListTests
/// </summary>
public class UpdateTodoListTests : TestBase
{
    /// <summary>
    /// ShouldRequireValidTodoListId
    /// </summary>
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateTodoListCommand { Id = Guid.NewGuid(), Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    /// <summary>
    /// ShouldRequireUniqueTitle
    /// </summary>
    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new CreateTodoListCommand
        {
            Title = "Other List"
        });

        (await FluentActions.Invoking(() =>
                    SendAsync(new UpdateTodoListCommand
                    {
                        Id = listId.Data.Id,
                        Title = "Other List"
                    }))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
            .And.Errors["Title"].Should().Contain("The specified title already exists.");
    }
    
    /// <summary>
    /// ShouldUpdateTodoList
    /// </summary>
    [Test]
    public async Task ShouldUpdateTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new UpdateTodoListCommand
        {
            Id = listId.Data.Id,
            Title = "Updated List Title"
        };

        await SendAsync(command);

        var list = Find<TodoList>(listId.Data.Id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.UpdatedBy.Should().NotBeNull();
        list.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        list.UpdatedDate.Should().NotBeNull();
        list.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}