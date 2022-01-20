using Microsoft.EntityFrameworkCore;
using ProductsArchive.Domain.Common;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<LocalizedString> LocalizedStrings { get; }
    DbSet<LocalizedStringValue> LocalizedStringValues { get; }

    DbSet<ProductCategory> ProductCategories { get; }
    DbSet<ProductGroup> ProductGroups { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductSize> ProductSizes { get; }

    void SetEntityState(AuditableEntity entity, EntityState entityState);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
