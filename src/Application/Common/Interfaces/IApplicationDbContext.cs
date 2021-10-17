// ------------------------------------------------------------------------------------
// IApplicationDbContext.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netca.Domain.Entities;

namespace netca.Application.Common.Interfaces
{
    /// <summary>
    /// IApplicationDbContext
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// TodoLists
        /// </summary>
        DbSet<TodoList> TodoLists { get; set; }
        
        /// <summary>
        /// TodoItems
        /// </summary>
        DbSet<TodoItem> TodoItems { get; set; }
        
        /// <summary>
        /// Changelogs
        /// </summary>
        DbSet<Changelog> Changelogs { get; set; }
        
        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
