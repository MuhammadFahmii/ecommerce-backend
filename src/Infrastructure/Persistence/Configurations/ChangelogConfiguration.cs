// ------------------------------------------------------------------------------------
// ChangelogConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ecommerce.Domain.Entities;

namespace ecommerce.Infrastructure.Persistence.Configurations;

/// <summary>
/// ChangelogConfiguration
/// </summary>
public class ChangelogConfiguration : IEntityTypeConfiguration<Changelog>
{
    /// <summary>
    /// Configure Changelog
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Changelog> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TableName)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);

        builder.Property(e => e.Method)
            .HasColumnType("varchar(6)")
            .HasMaxLength(6);

        builder.Property(e => e.KeyValues)
            .HasColumnType("text");

        builder.Property(e => e.NewValues)
            .HasColumnType("text");

        builder.Property(e => e.OldValues)
            .HasColumnType("text");

        builder.Property(e => e.ChangeBy)
            .HasColumnType("text");

        builder.Property(e => e.ChangeDate)
            .HasColumnType("integer");
    }
}