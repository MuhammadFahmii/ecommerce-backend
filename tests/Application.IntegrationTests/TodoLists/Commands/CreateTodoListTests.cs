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
using netca.Application.IntegrationTests.Data;
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
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldRequireMinimumFields))]
    public async Task ShouldRequireMinimumFields(Guid id, string title)
    {
        var command = new CreateTodoListCommand{Id = id, Title = title};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    /// <summary>
    /// ShouldCreateTodoList
    /// </summary>
    [Test]
    [TestCaseSource(typeof(TodoListDataTest), nameof(TodoListDataTest.ShouldCreated))]
    public async Task ShouldCreateTodoList(Guid id, string  title)
    {
        await SendAsync(new CreateTodoListCommand{Id = id, Title = title});
        var list = Find<TodoList>(id);
        list.Should().NotBeNull();
        list!.Title.Should().Be(title);
        list.CreatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        list.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 10000);
    }
}
