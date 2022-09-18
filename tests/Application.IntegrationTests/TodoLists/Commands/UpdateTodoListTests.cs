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
using netca.Application.IntegrationTests.Data;
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
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldRequireValidTodoListId))]
    public async Task ShouldRequireValidTodoListId(Guid id, string title, bool check)
    {
        if (check)
        {
            var command = new UpdateTodoListCommand { Id = id, Title = title };
            await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }
        else
        {
            var command = new UpdateTodoListCommand{Id = id, Title = title};
            await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }


    }
    
    /// <summary>
    /// ShouldRequireUniqueTitle
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldCreated))]
    public async Task ShouldRequireUniqueTitle(Guid id, string title)
    {
        await SendAsync(new CreateTodoListCommand
        {
            Id = id,
            Title = title
        });
        var newId = Guid.NewGuid();
        var nweTitle = $"{title} unique check";
        await SendAsync(new CreateTodoListCommand
        {
            Id = newId,
            Title = nweTitle
        });

        (await FluentActions.Invoking(() =>
                    SendAsync(new UpdateTodoListCommand
                    {
                        Id = id,
                        Title = nweTitle
                    }))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
            .And.Errors["Title"].Should().Contain("The specified title already exists.");
    }
    
    /// <summary>
    /// ShouldUpdateTodoList
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldCreated))]
    public async Task ShouldUpdateTodoList(Guid id, string title)
    {
        await SendAsync(new CreateTodoListCommand
        {
            Id = id,
            Title = title
        });

        var command = new UpdateTodoListCommand
        {
            Id = id,
            Title = $"Updated List {title}"
        };

        await SendAsync(command);

        var list = Find<TodoList>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.UpdatedBy.Should().NotBeNull();
        list.UpdatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);

    }
}