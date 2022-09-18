// ------------------------------------------------------------------------------------
// UpdateTodoItemDetailTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;
using netca.Application.IntegrationTests.Data;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using netca.Application.TodoItems.Commands.UpdateTodoItemDetail;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using netca.Domain.Enums;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

/// <summary>
/// UpdateTodoItemDetailTests
/// </summary>
public class UpdateTodoItemDetailTests : TestBase
{
    /// <summary>
    /// ShouldRequireValidTodoItemId
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoItemDataTests), nameof(TodoItemDataTests.ShouldRequireMinimumFieldsUpdateDetail))]
    public async Task ShouldRequireValidTodoItemId(Guid id, Guid listId, PriorityLevel priority)
    {
        var command = new UpdateTodoItemDetailCommand { Id = id, ListId = listId, Priority = priority };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();

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
        
        var command = new UpdateTodoItemDetailCommand
        {
            Id = id,
            ListId = listId,
            Note = "This is the note.",
            Priority = PriorityLevel.High
        };
    
        await SendAsync(command);
    
        var item = Find<TodoItem>(id);
    
        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Note.Should().Be(command.Note);
        item.Priority.Should().Be(command.Priority);
        item.UpdatedBy.Should().NotBeNull();
        item.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.UpdatedDate.Should().NotBeNull();
        item.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 10000);
    }
}
