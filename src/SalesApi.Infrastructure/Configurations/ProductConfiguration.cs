using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products"); 

            builder.HasKey(p => p.Id); 

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100); 

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)"); 

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Category)
                .HasMaxLength(100);

            builder.Property(p => p.Image)
                .HasMaxLength(255);
        }
    }
}
