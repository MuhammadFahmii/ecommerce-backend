// ------------------------------------------------------------------------------------
// IApplicationDbContext.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using netca.Domain.Entities;

namespace netca.Application.Common.Interfaces
{
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
        /// Gets database
        /// </summary>
        public DatabaseFacade Database { get; }

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
