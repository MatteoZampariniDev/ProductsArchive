using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Infrastructure.Persistence.Configurations.ProductsSection;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.GroupId);

        builder.HasOne(x => x.Size)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SizeId);
    }
}
