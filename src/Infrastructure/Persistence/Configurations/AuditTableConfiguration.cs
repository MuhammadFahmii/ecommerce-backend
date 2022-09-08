// ------------------------------------------------------------------------------------
// AuditTableConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netca.Domain.Common;

namespace netca.Infrastructure.Persistence.Configurations;

/// <summary>
/// AuditTableConfiguration
/// </summary>
/// <typeparam name="TBase"></typeparam>
public abstract class AuditTableConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : BaseAuditableEntity
{
    /// <summary>
    /// Configure for all entities
    /// </summary>
    /// <param name="builder"></param>
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("Uniqueidentifier")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IsDeleted)
            .HasColumnType("bit");

        builder.Property(e => e.CreatedBy)
            .HasColumnType("Uniqueidentifier");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("bigint");

        builder.Property(e => e.UpdatedBy)
            .HasColumnType("Uniqueidentifier");

        builder.Property(e => e.UpdatedDate)
            .HasColumnType("bigint");

        builder.Property(e => e.IsDeleted)
            .HasColumnType("bit");

    }
}