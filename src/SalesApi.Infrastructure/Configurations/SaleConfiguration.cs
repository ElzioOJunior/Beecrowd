using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale"); 
            builder.HasKey(s => s.Id); 

            builder.Property(s => s.SaleNumber)
                .IsRequired()
                .HasMaxLength(20); 

            builder.Property(s => s.SaleDate)
                .IsRequired(); 

            builder.Property(s => s.CustomerId)
                .IsRequired(); 

            builder.Property(s => s.BranchId)
                .IsRequired();

            builder.Property(s => s.TotalAmount).HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(s => s.Canceled)
                .HasDefaultValue(false);

            builder.HasMany(s => s.Items)
                .WithOne(i => i.Sale)
                .HasForeignKey(i => i.SaleId); 
        }
    }
}
