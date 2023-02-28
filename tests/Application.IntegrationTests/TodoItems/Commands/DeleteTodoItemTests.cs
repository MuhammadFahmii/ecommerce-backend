// ------------------------------------------------------------------------------------
// DeleteTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ecommerce.Application.Common.Exceptions;
using ecommerce.Application.IntegrationTests.Data;
using ecommerce.Application.TodoItems.Commands.CreateTodoItem;
using ecommerce.Application.TodoItems.Commands.DeleteTodoItem;
using ecommerce.Application.TodoLists.Commands.CreateTodoList;
using ecommerce.Domain.Entities;
using NUnit.Framework;

namespace ecommerce.Application.IntegrationTests.TodoItems.Commands;
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
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.Delete))]
    public async Task ShouldRequireValidTodoItemId(Guid id, bool check)
    {
        var command = new DeleteTodoItemCommand { Id = id};
        if (check)
        {
            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }
        else
        {
            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
    }

    /// <summary>
    /// ShouldDeleteTodoItem
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldCreated))]
    public async Task ShouldDeleteTodoItem(Guid id, Guid listId, string title)
    {
        await SendAsync(new CreateTodoListCommand
        {
            Id = listId,
            Title = $"{title} List"
        });

        await SendAsync(new CreateTodoItemCommand
        {
            Id = id,
            ListId = listId,
            Title = title
        });

        await SendAsync(new DeleteTodoItemCommand
        {
            Id = id
        });

        var item =  Find<TodoItem>(id);

        item.Should().BeNull();
    }
}