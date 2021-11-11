// ------------------------------------------------------------------------------------
// ApplicationDbContext.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using netca.Application.Common.Interfaces;
using netca.Domain.Common;
using netca.Domain.Entities;
using Newtonsoft.Json;

namespace netca.Infrastructure.Persistence
{
    /// <summary>
    /// ApplicationDbContext
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="domainEventService"></param>
        /// <param name="dateTime"></param>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService, IDateTime dateTime) : base(options)
        {
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        /// <summary>
        /// Gets or sets todoItems
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }

        /// <summary>
        /// Gets or sets changelogs
        /// </summary>
        public DbSet<Changelog> Changelogs { get; set; }

        /// <summary>
        /// Gets or sets todoLists
        /// </summary>
        public DbSet<TodoList> TodoLists { get; set; }

        /// <summary>
        /// to prevent hard delete
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Exception</exception>
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// to prevent hard delete
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="NotImplementedException">Exception</exception>
        public override void RemoveRange(params object[] entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// to prevent hard delete
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="NotImplementedException">Exception</exception>
        public override void RemoveRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// to prevent hard delete
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Exception</exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changelogEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(changelogEntries, cancellationToken);
            await DispatchEvents();
            return result;
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker
                    .Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

                if (domainEventEntity == null)
                    break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }

        private List<ChangelogEntry> OnBeforeSaveChanges()
        {
            var ignoreTable = new List<string> { };

            ChangeTracker.DetectChanges();
            var changelogEntries = new List<ChangelogEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Changelog || entry.State is EntityState.Detached or EntityState.Unchanged)
                    continue;

                var changelogEntry = new ChangelogEntry(entry)
                {
                    ChangeDate = _dateTime.UtcNow,
                    TableName = entry.Metadata.GetTableName()
                };

                if (!ignoreTable.Contains(changelogEntry.TableName))
                {
                    changelogEntries.Add(changelogEntry);

                    ChangelogEntryEvent(changelogEntry, entry);
                }
            }

            foreach (var changelogEntry in changelogEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Changelogs.Add(changelogEntry.ToAudit());
            }

            return changelogEntries
                .Where(_ => _.HasTemporaryProperties)
                .ToList();
        }

        private static void ChangelogEntryEvent(ChangelogEntry changelogEntry, EntityEntry entry)
        {
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    changelogEntry.TemporaryProperties.Add(property);
                    continue;
                }

                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    changelogEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        changelogEntry.NewValues[propertyName] = property.CurrentValue;
                        changelogEntry.Method = "ADD";
                        break;

                    case EntityState.Deleted:
                        changelogEntry.OldValues[propertyName] = property.OriginalValue;
                        changelogEntry.Method = "DELETE";
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            changelogEntry.OldValues[propertyName] = property.OriginalValue;
                            changelogEntry.NewValues[propertyName] = property.CurrentValue;
                            changelogEntry.Method = "EDIT";
                        }

                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(entry.State.ToString(), "Un know entry.State.");
                }
            }
        }

        private Task OnAfterSaveChanges(List<ChangelogEntry> changelogEntries, CancellationToken cancellationToken)
        {
            if (changelogEntries == null || changelogEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var changelogEntry in changelogEntries)
            {
                foreach (var prop in changelogEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                        changelogEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    else
                        changelogEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }

                Changelogs.Add(changelogEntry.ToAudit());
            }

            return SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// ChangelogEntry
    /// </summary>
    public class ChangelogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogEntry"/> class.
        /// </summary>
        /// <param name="entry"></param>
        public ChangelogEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// Gets entry
        /// </summary>
        /// <value></value>
        private EntityEntry Entry { get; }

        /// <summary>
        /// Gets or sets method
        /// </summary>
        /// <value></value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets tableName
        /// </summary>
        /// <value></value>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets changeDate
        /// </summary>
        /// <value></value>
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Gets keyValues
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> KeyValues { get; } = new();

        /// <summary>
        /// Gets oldValues
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> OldValues { get; } = new();

        /// <summary>
        /// Gets newValues
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> NewValues { get; } = new();

        /// <summary>
        /// Gets temporaryProperties
        /// </summary>
        /// <returns></returns>
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        /// <summary>
        /// Gets a value indicating whether hasTemporaryProperties
        /// </summary>
        /// <returns></returns>
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        /// <summary>
        /// ToAudit save record for audit changelog
        /// </summary>
        /// <returns></returns>
        public Changelog ToAudit()
        {
            var changelog = new Changelog
            {
                TableName = TableName,
                ChangeDate = ChangeDate,
                Method = Method,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
            };

            return changelog;
        }
    }
}