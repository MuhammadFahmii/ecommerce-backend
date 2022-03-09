// ------------------------------------------------------------------------------------
// CreateTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using netca.Application.Common.Models;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoItems.Commands;

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
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// ShouldCreateTodoItem
    /// </summary>
    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new CreateTodoItemCommand
        {
            ListId = listId.Data.Id,
            Title = "Tasks"
        };

        var itemId = await SendAsync(command);
        var list = Find<TodoList>(listId.Data.Id);
        list.Should().NotBeNull();
        list?.Id.Should().Be(listId.Data.Id);
        var item =  Find<TodoItem>(itemId.Data.Id);
        
        item.Should().NotBeNull();
        item!.ListId.Should().Be(command.ListId);
        item.Title.Should().Be("Tasks");
        item.CreatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        item.CreatedDate.Should().NotBeNull();
        item.UpdatedBy.Should().BeNull();
        item.UpdatedDate.Should().BeNull();
    }
}