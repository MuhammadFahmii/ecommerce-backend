// ------------------------------------------------------------------------------------
// DeleteTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using netca.Application.TodoItems.Commands.DeleteTodoItem;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoItems.Commands;
using static Testing;

/// <summary>
/// DeleteTodoItemTests
/// </summary>
public class DeleteTodoItemTests : TestBase
{
    /// <summary>
    /// ShouldRequireValidTodoItemId
    /// </summary>
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand { Id = Guid.NewGuid() };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    /// ShouldDeleteTodoItem
    /// </summary>
    [Test]
    public async Task ShouldDeleteTodoItem()
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

        await SendAsync(new DeleteTodoItemCommand
        {
            Id = itemId.Data.Id
        });

        var item =  Find<TodoItem>(itemId.Data.Id
        );

        item.Should().BeNull();
    }
}