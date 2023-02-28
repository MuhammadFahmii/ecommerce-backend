// ------------------------------------------------------------------------------------
// CreateTodoItemTests.cs  2022
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
using ecommerce.Application.TodoLists.Commands.CreateTodoList;
using ecommerce.Domain.Entities;
using NUnit.Framework;
using Shouldly;

namespace ecommerce.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

/// <summary>
/// CreateTodoItemTests
/// </summary>
public class CreateTodoItemTests : TestBase
{
    /// <summary>
    /// ShouldRequireMinimumFields
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldRequireMinimumFields))]
    public async Task ShouldRequireMinimumFields(Guid id, Guid listId, string title)
    {
        var command = new CreateTodoItemCommand{ListId = listId, Id = id, Title = title};

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// ShouldCreateTodoItem
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldCreated))]
    public async Task ShouldCreateTodoItem(Guid id, Guid listId, string title)
    {
         await SendAsync(new CreateTodoListCommand
        {
            Id = listId,
            Title = $"{title} List"
        });

        var command = new CreateTodoItemCommand
        {
            Id = id,
            ListId = listId,
            Title = title
        };

        await SendAsync(command);
        var list = Find<TodoList>(listId);
        list.Should().NotBeNull();
        list?.Id.Should().Be(listId);
        var item =  Find<TodoItem>(id);
        
        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Title.Should().Be(title);
        item.CreatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.CreatedDate.Should().ShouldNotBeNull();
        item.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 10000);
        item.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.UpdatedDate.Should().NotBeNull();
    }
}