using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Infrastructure.Persistence.Configurations;

/// <summary>
/// Voucher Configuration
/// </summary>
public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
    }
}
