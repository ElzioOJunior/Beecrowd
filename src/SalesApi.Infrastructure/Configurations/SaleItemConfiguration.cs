using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItem");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);

            builder.HasOne(i => i.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SaleId);

            builder.Property(i => i.Discount)
                  .IsRequired()
                  .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Total)
                  .IsRequired()
                  .HasColumnType("decimal(10,2)");

            builder.Property(s => s.Canceled)
                .HasDefaultValue(false);
        }
    }
}
