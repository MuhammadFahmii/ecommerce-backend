// ------------------------------------------------------------------------------------
// ChangelogConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netca.Domain.Entities;

namespace netca.Infrastructure.Persistence.Configurations
{
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
                .HasColumnType("Uniqueidentifier")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.TableName)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            builder.Property(e => e.Method)
                .HasColumnType("nvarchar(6)")
                .HasMaxLength(6);

            builder.Property(e => e.KeyValues)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);
            builder.Property(e => e.NewValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.OldValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.ChangeDate)
                .HasColumnType("datetime2");
        }
    }
}
