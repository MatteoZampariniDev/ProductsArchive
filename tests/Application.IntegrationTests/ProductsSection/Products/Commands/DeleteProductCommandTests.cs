using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.Products.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.Products.Commands;

public class DeleteProductCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new DeleteProductCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        var product = new Product
        {
            Size = new ProductSize(),
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            }
        };
        await AddAsync(product);

        var command = new DeleteProductCommand(product.Id);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();

        var command = new DeleteProductCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldDeleteProductAsync()
    {
        await RunAsAdministratorAsync();

        var product = new Product
        {
            Size = new ProductSize(),
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            }
        };
        await AddAsync(product);

        await SendAsync(new DeleteProductCommand(product.Id));

        var productEntity = await FindAsync<Product>(product.Id);
        var sizeEntity = await FindAsync<ProductSize>(product.Size.Id);
        var groupEntity = await FindAsync<ProductGroup>(product.Group.Id);

        productEntity.Should().BeNull();
        sizeEntity.Should().NotBeNull();
        groupEntity.Should().NotBeNull();
    }
    #endregion
}
