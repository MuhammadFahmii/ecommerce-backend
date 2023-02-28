// ------------------------------------------------------------------------------------
// TodoListConfiguration.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ecommerce.Domain.Entities;
using ecommerce.Domain.ValueObjects;

namespace ecommerce.Infrastructure.Persistence.Configurations;

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