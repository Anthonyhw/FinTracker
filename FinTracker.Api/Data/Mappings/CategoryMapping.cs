using FinTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTracker.Api.Data.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Description)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(160);
        }
    }
}
