// ------------------------------------------------------------------------------------
// CreateTodoListTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoLists.Commands;
using static Testing;

/// <summary>
/// CreateTodoListTests
/// </summary>
public class CreateTodoListTests : TestBase
{
    /// <summary>
    /// ShouldRequireMinimumFields
    /// </summary>
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoListCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// ShouldRequireUniqueTitle
    /// </summary>
    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        await SendAsync(new CreateTodoListCommand
        {
            Title = "Shopping"
        });

        var command = new CreateTodoListCommand
        {
            Title = "Shopping"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// ShouldCreateTodoList
    /// </summary>
    [Test]
    public async Task ShouldCreateTodoList()
    {

        var command = new CreateTodoListCommand
        {
            Title = "Tasks"
        };

        var listId = await SendAsync(command);

        var list = Find<TodoList>(listId.Data.Id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.CreatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        list.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
