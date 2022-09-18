// ------------------------------------------------------------------------------------
// DeleteTodoListTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.IntegrationTests.Data;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Application.TodoLists.Commands.DeleteTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

/// <summary>
/// DeleteTodoListTests
/// </summary>
public class DeleteTodoListTests : TestBase
{
    /// <summary>
    /// ShouldRequireValidTodoListId
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldCreated))]
    public async Task ShouldRequireValidTodoListId(Guid id, string title)
    {
        var command = new DeleteTodoListCommand{Id = id};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    /// <summary>
    /// ShouldDeleteTodoList
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldCreated))]
    public async Task ShouldDeleteTodoList(Guid id, string title)
    {
        await SendAsync(new CreateTodoListCommand
        {
            Id = id,
            Title = title
        });

        await SendAsync(new DeleteTodoListCommand
        {
            Id = id
        });

        var list = Find<TodoList>(id);

        list.Should().BeNull();
    }
}