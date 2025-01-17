using FinTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTracker.Api.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired(true)
                .HasColumnType("VARCHAR");

            builder.Property(x => x.ExternalReference)
                .IsRequired(false)
                .HasColumnType("VARCHAR")
                .HasMaxLength(60);

            builder.Property(x => x.Gateway)
                .IsRequired(true)
                .HasColumnType("SMALLINT");

            builder.Property(x => x.CreatedAt)
                .IsRequired(true)
                .HasColumnType("DATETIME2");

            builder.Property(x => x.UpdatedAt)
                .IsRequired(true)
                .HasColumnType("DATETIME2");

            builder.Property(x => x.Status)
                .IsRequired(true)
                .HasColumnType("SMALLINT");

            builder.Property(x => x.UserId)
                .IsRequired(true)
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);

            builder.HasOne(x => x.Product)
                .WithMany();
            
            builder.HasOne(x => x.Voucher)
                .WithMany();
        }
    }
}
