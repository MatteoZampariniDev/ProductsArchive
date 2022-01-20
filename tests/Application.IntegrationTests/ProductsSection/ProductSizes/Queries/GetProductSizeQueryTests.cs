using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductSizes.Queries;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductSizes.Queries;

public class GetProductSizeQueryTests : TestBase
{
    #region Validation

    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        var size = new ProductSize();
        await AddAsync(size);

        var command = new GetProductSizeQuery(size.Id);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        var command = new GetProductSizeQuery(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Query
    [Test]
    public async Task ShouldReturnProductSizeAsync()
    {
        var culture = RunWithDefaultCulture();

        var size = new ProductSize
        {
            Name = LocalizedString.Create(culture, "Size")
        };
        await AddAsync(size);

        var queryResult = await SendAsync(new GetProductSizeQuery(size.Id));

        queryResult.Should().NotBeNull();
        queryResult.Id.Should().Be(size.Id);
        queryResult.Name.Should().Be("Size");
    }
    #endregion
}
