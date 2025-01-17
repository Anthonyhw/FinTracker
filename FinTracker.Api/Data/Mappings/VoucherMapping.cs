using FinTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTracker.Api.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");
            
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired(true)
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Title)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Amount)
                .IsRequired(true)
                .HasColumnType("MONEY");

            builder.Property(x => x.IsActive)
                .IsRequired(true)
                .HasColumnType("BIT");
        }
    }
}
