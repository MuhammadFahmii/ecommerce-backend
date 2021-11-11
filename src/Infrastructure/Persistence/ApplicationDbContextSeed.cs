// ------------------------------------------------------------------------------------
// ApplicationDbContextSeed.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netca.Domain.Entities;
using netca.Domain.ValueObjects;

namespace netca.Infrastructure.Persistence
{
    /// <summary>
    /// ApplicationDbContextSeed
    /// </summary>
    public static class ApplicationDbContextSeed
    {
        /// <summary>
        /// SeedSampleDataAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            if (!context.TodoLists.IgnoreQueryFilters().Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Colour = Colour.Blue,
                    DeletedDate = null,
                    IsActive = true,
                    Items =
                    {
                        new TodoItem
                        {
                            Title = "Apples", Done = true, DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Milk", Done = true, DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Bread", Done = true, DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Toilet paper", DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Pasta", DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Tissues", DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Tuna", DeletedDate = null,
                            IsActive = true
                        },
                        new TodoItem
                        {
                            Title = "Water", DeletedDate = null,
                            IsActive = true
                        }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}