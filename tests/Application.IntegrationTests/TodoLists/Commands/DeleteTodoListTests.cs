// ------------------------------------------------------------------------------------
// DeleteTodoListTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
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
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand { Id = Guid.NewGuid() };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    /// <summary>
    /// ShouldDeleteTodoList
    /// </summary>
    [Test]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand
        {
            Id = listId.Data.Id
        });

        var list = Find<TodoList>(listId.Data.Id);

        list.Should().BeNull();
    }
}