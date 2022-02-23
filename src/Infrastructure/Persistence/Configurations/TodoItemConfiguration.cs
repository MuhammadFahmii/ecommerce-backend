// ------------------------------------------------------------------------------------
// TodoItemConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netca.Domain.Entities;

namespace netca.Infrastructure.Persistence.Configurations;

/// <summary>
/// TodoItemConfiguration
/// </summary>
public class TodoItemConfiguration : AuditTableConfiguration<TodoItem>
{
    /// <summary>
    /// Configure TodoItem
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        base.Configure(builder);
        builder.Ignore(e => e.DomainEvents);

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
        base.Configure(builder);
    }
}