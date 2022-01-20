using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductCategories.Commands;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductCategories.Commands;

using static ProductsSectionTesting;

public class UpdateProductCategoryCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new UpdateProductCategoryCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        var category = new ProductCategory();
        await AddAsync(category);

        var command = new UpdateProductCategoryCommand
        {
            Id = category.Id
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();
        var command = new UpdateProductCategoryCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldUpdateProductCategoryAsync()
    {
        var culture = RunWithDefaultCulture();
        var user = await RunAsAdministratorAsync();

        var category = new ProductCategory
        {
            Name = LocalizedString.Create(culture, "")
        };
        await AddAsync(category);

        await SendAsync(new UpdateProductCategoryCommand
        {
            Id = category.Id,
            Name = "Category"
        });

        var entity = await GetProductCategoryAsync(category.Id);

        entity.Should().NotBeNull();
        entity!.Id.Should().Be(category.Id);
        entity.Name.Should().NotBeNull();
        entity.Name[culture].Should().Be("Category");
        entity.LastModifiedBy.Should().Be(user);
        entity.LastModified.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
    }
    #endregion
}