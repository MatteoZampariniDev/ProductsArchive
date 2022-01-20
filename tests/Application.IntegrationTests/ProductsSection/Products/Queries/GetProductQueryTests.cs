using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.Products.Queries;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.Products.Queries;

public class GetProductQueryTests : TestBase
{
    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        // arrange
        var product = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product);

        var command = new GetProductQuery(product.Id);

        // act - assert
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        var command = new GetProductQuery(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Query
    [Test]
    public async Task ShouldReturnProductAsync()
    {
        // arrange
        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        var product = new Product
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        };
        await AddAsync(product);

        // act
        var queryResult = await SendAsync(new GetProductQuery(product.Id));

        // assert
        queryResult.Should().NotBeNull();
        queryResult!.Id.Should().Be(product.Id);
        queryResult.Group.Should().NotBeNull();
        queryResult.Group!.Id.Should().Be(group.Id);
        queryResult.Size.Should().NotBeNull();
        queryResult.Size!.Id.Should().Be(size.Id);
        queryResult.ProductId.Should().Be("00000");
    }
    #endregion
}
