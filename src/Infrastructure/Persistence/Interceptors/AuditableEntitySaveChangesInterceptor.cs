// ------------------------------------------------------------------------------------
// AuditableEntitySaveChangesInterceptor.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using netca.Application.Common.Interfaces;
using netca.Domain.Common;

namespace netca.Infrastructure.Persistence.Interceptors;


/// <summary>
/// AuditableEntitySaveChangesInterceptor
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserAuthorizationService _currentUserService;
    private readonly IDateTime _dateTime;
    
    /// <summary>
    /// AuditableEntitySaveChangesInterceptor
    /// </summary>
    /// <param name="currentUserService"></param>
    /// <param name="dateTime"></param>
    public AuditableEntitySaveChangesInterceptor(
        IUserAuthorizationService currentUserService,
        IDateTime dateTime)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    /// <summary>
    /// SavingChanges
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    
    /// <summary>
    /// SavingChangesAsync
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    /// <summary>
    /// UpdateEntities
    /// </summary>
    /// <param name="context"></param>
    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            var userId = _currentUserService.GetUserId();
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedDate = _dateTime.UtcNow;
                entry.Entity.IsDeleted = false;
            }

            if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
                !entry.HasChangedOwnedEntities()) continue;
            entry.Entity.UpdatedBy = userId;
            entry.Entity.UpdatedDate = _dateTime.UtcNow;
        }
    }
}

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// HasChangedOwnedEntities
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}