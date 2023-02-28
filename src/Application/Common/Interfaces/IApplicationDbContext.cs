// ------------------------------------------------------------------------------------
// IApplicationDbContext.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ecommerce.Domain.Entities;

namespace ecommerce.Application.Common.Interfaces;

/// <summary>
/// IApplicationDbContext
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets todoLists
    /// </summary>
    DbSet<TodoList> TodoLists { get; set; }

    /// <summary>
    /// Gets or sets todoItems
    /// </summary>
    DbSet<TodoItem> TodoItems { get; set; }

    /// <summary>
    /// Gets or sets changelogs
    /// </summary>
    DbSet<Changelog> Changelogs { get; set; }

    /// <summary>
    /// Gets or sets products
    /// </summary>
    DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets orders
    /// </summary>
    DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Gets or sets product orders
    /// </summary>
    DbSet<OrderProduct> OrderProducts { get; set; }

    /// <summary>
    /// Gets database
    /// </summary>
    public DatabaseFacade Database { get; }

    /// <summary>
    /// AsNoTracking
    /// </summary>
    public void AsNoTracking();

    /// <summary>
    /// Clear
    /// </summary>
    public void Clear();

    /// <summary>
    /// Execute using EF Core resiliency strategy
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task ExecuteResiliencyAsync(Func<Task> action);

    /// <summary>
    /// SaveChangesAsync
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
