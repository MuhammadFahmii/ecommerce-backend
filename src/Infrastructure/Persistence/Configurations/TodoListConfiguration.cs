// ------------------------------------------------------------------------------------
// TodoListConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using netca.Domain.Entities;
using netca.Domain.ValueObjects;

namespace netca.Infrastructure.Persistence.Configurations;

/// <summary>
/// TodoListConfiguration
/// </summary>
public class TodoListConfiguration : AuditTableConfiguration<TodoList>
{
    /// <summary>
    /// Configure TodoList
    /// </summary>
    /// <param name="builder"></param>
    public override void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
        builder.OwnsOne(t => t.Colour);
        base.Configure(builder);
    }
}