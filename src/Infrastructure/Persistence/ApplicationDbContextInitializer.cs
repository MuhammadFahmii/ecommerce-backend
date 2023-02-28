// ------------------------------------------------------------------------------------
// ApplicationDbContextInitializer.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ecommerce.Domain.Entities;
using ecommerce.Domain.ValueObjects;

namespace ecommerce.Infrastructure.Persistence;

/// <summary>
/// ApplicationDbContextInitializer
/// </summary>
public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContextInitializer"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// InitializeAsync
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                _logger.LogWarning("Migrating Database");
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }

    /// <summary>
    /// SeedAsync
    /// </summary>
    /// <returns></returns>
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    /// <summary>
    /// TrySeedAsync
    /// </summary>
    private async Task TrySeedAsync()
    {
        if (!_context.TodoLists.IgnoreQueryFilters().Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Shopping",
                Colour = Colour.Blue,
                Items =
                {
                    new TodoItem
                    {
                        Title = "Apples", Done = true,
                    },
                    new TodoItem
                    {
                        Title = "Milk", Done = true,
                    },
                    new TodoItem
                    {
                        Title = "Bread", Done = true,
                    },
                    new TodoItem
                    {
                        Title = "Toilet paper",
                    },
                    new TodoItem
                    {
                        Title = "Pasta",
                    },
                    new TodoItem
                    {
                        Title = "Tissues",
                    },
                    new TodoItem
                    {
                        Title = "Tuna",
                    },
                    new TodoItem
                    {
                        Title = "Water",
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
