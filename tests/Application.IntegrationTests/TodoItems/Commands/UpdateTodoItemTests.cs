// ------------------------------------------------------------------------------------
// UpdateTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ecommerce.Application.Common.Exceptions;
using ecommerce.Application.Common.Models;
using ecommerce.Application.IntegrationTests.Data;
using ecommerce.Application.TodoItems.Commands.CreateTodoItem;
using ecommerce.Application.TodoItems.Commands.UpdateTodoItem;
using ecommerce.Application.TodoLists.Commands.CreateTodoList;
using ecommerce.Domain.Entities;
using NUnit.Framework;

namespace ecommerce.Application.IntegrationTests.TodoItems.Commands;
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
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldCreated))]
    public async Task ShouldRequireValidTodoItemId(Guid id, Guid listId, string title)
    {
        var command = new UpdateTodoItemCommand { Id = id, Title = title};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    /// <summary>
    /// ShouldUpdateTodoItem
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldCreated))]
    public async Task ShouldUpdateTodoItem(Guid id, Guid listId, string title)
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

        var command = new UpdateTodoItemCommand
        {
            Id = id,
            Title = "Updated Item Title"
        };

        await SendAsync(command);

        var item =  Find<TodoItem>(id);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.UpdatedBy.Should().NotBeNull();
        item.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.UpdatedDate.Should().NotBeNull();
        item.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 10000);
    }
}