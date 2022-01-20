using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Infrastructure.Persistence.Configurations.ProductsSection;

public class ProductGroupConfig : IEntityTypeConfiguration<ProductGroup>
{
    public void Configure(EntityTypeBuilder<ProductGroup> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.CategoryId);
    }
}
