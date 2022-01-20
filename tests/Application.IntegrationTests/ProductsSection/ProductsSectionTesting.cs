using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsArchive.Application.Common.Extensions;
using ProductsArchive.Domain.Entities.ProductsSection;
using ProductsArchive.Infrastructure.Persistence;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection;

public class ProductsSectionTesting : Testing
{
    public static async Task<ProductCategory?> GetProductCategoryAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();    

        return await context!.ProductCategories
            .Where(x => x.Id == id)
            .IncludeLocalizedProperty(x => x.Name!)
            .SingleOrDefaultAsync();
    }

    public static async Task<ProductSize?> GetProductSizeAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context!.ProductSizes
            .Where(x => x.Id == id)
            .IncludeLocalizedProperty(x => x.Name!)
            .SingleOrDefaultAsync();
    }

    public static async Task<ProductGroup?> GetProductGroupAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context!.ProductGroups
            .Where(x => x.Id == id)
            .IncludeLocalizedProperty(x => x.Name!)
            .IncludeLocalizedProperty(x => x.Description!)
            .SingleOrDefaultAsync();
    }
}