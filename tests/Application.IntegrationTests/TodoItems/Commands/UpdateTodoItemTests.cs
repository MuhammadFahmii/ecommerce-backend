// ------------------------------------------------------------------------------------
// UpdateTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using netca.Application.TodoItems.Commands.UpdateTodoItem;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoItems.Commands;
using static Testing;

/// <summary>
/// UpdateTodoItemTests
/// </summary>
public class UpdateTodoItemTests : TestBase
{
    /// <summary>
    /// ShouldRequireValidTodoItemId
    /// </summary>
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new UpdateTodoItemCommand { Id = Guid.NewGuid(), Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    /// <summary>
    /// ShouldUpdateTodoItem
    /// </summary>
    [Test]
    public async Task ShouldUpdateTodoItem()
    {

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId.Data.Id,
            Title = "New Item"
        });

        var command = new UpdateTodoItemCommand
        {
            Id = itemId.Data.Id,
            Title = "Updated Item Title"
        };

        await SendAsync(command);

        var item =  Find<TodoItem>(itemId.Data.Id);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.UpdatedBy.Should().NotBeNull();
        item.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.UpdatedDate.Should().NotBeNull();
        item.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}