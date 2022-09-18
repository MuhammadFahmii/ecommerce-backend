// ------------------------------------------------------------------------------------
// GetTodosTests.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using netca.Application.TodoLists.Queries.GetTodos;
using netca.Domain.Entities;
using netca.Domain.ValueObjects;
using NUnit.Framework;

namespace netca.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

/// <summary>
/// GetTodosTests
/// </summary>
public class GetTodosTests : TestBase
{
    /// <summary>
    /// ShouldReturnPriorityLevels
    /// </summary>
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Meta["PriorityLevels"].Should().NotBeEmpty();
    }
    
    /// <summary>
    /// ShouldReturnAllListsAndItems
    /// </summary>
    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
            {
                new TodoItem { Title = "Apples", Done = true },
                new TodoItem { Title = "Milk", Done = true },
                new TodoItem { Title = "Bread", Done = true },
                new TodoItem { Title = "Toilet paper" },
                new TodoItem { Title = "Pasta" },
                new TodoItem { Title = "Tissues" },
                new TodoItem { Title = "Tuna" }
            }
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Data.Should().HaveCount(1);
        (result.Data ?? throw new InvalidOperationException()).First().Items.Should().HaveCount(7);
    }
}