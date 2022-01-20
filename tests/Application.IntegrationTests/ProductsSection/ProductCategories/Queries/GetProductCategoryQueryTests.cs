using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductCategories.Queries;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductCategories.Queries;

public class GetProductCategoryQueryTests : TestBase
{
    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        var category = new ProductCategory();
        await AddAsync(category);

        var command = new GetProductCategoryQuery(category.Id);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        var command = new GetProductCategoryQuery(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Query
    [Test]
    public async Task ShouldReturnProductCategoryAsync()
    {
        var culture = RunWithDefaultCulture();

        var category = new ProductCategory
        {
            Name = LocalizedString.Create(culture, "Category")
        };
        await AddAsync(category);

        var queryResult = await SendAsync(new GetProductCategoryQuery(category.Id));

        queryResult.Should().NotBeNull();
        queryResult!.Id.Should().Be(category.Id);
        queryResult.Name.Should().Be("Category");
    }
    #endregion
}
