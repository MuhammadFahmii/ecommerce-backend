// ------------------------------------------------------------------------------------
// CreateTodoItemTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using netca.Application.Common.Models;
using netca.Application.TodoItems.Commands.CreateTodoItem;
using netca.Application.TodoLists.Commands.CreateTodoList;
using netca.Domain.Entities;
using netca.Infrastructure.Persistence;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

/// <summary>
/// CreateTodoItemTests
/// </summary>
public class CreateTodoItemTests : TestBase
{
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
        // var listId = await SendAsync(new CreateTodoListCommand
        // {
        //     Title = "New List1"
        // });
        //
        // var command = new CreateTodoItemCommand
        // {
        //     ListId = listId.Data.Id,
        //     Title = "Tasks"
        // };
        
        using var scope = ScopeFactory?.CreateScope();

        var context = scope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
         context?.TodoLists.Add(new TodoList
        {
            Title = "New List1"
        });
        await context?.SaveChangesAsync()!;
        var ch = context.Changelogs.ToList();
        var list = context.TodoLists.ToList();
        var item = context.TodoItems.ToList();

        //var itemId = await SendAsync(command);
        // var list = Find<TodoItem>(itemId.Data.Id);
        // list.Should().NotBeNull();
        // list?.Id.Should().Be(listId.Data.Id);
        // var item =  Find<TodoItem>(itemId.Data.Id);
        //
        // item.Should().NotBeNull();
        // item!.ListId.Should().Be(command.ListId);
        // item.Title.Should().Be("Tasks");
        // item.CreatedBy.Should().Be(MockData.GetAuthorizedUser().UserId);
        // item.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        // item.UpdatedBy.Should().Be(null);
        // item.UpdatedDate.Should().BeNull();
    }
}