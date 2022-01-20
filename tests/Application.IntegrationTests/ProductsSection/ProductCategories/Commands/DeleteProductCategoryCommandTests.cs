using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductCategories.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductCategories.Commands;

public class DeleteProductCategoryCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new DeleteProductCategoryCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        // arrange
        var category = new ProductCategory();
        await AddAsync(category);

        var command = new DeleteProductCategoryCommand(category.Id);

        // act - assert
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();
        var command = new DeleteProductCategoryCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldDeleteCategoryAsync()
    {
        await RunAsAdministratorAsync();
        var category = new ProductCategory();
        await AddAsync(category);

        await SendAsync(new DeleteProductCategoryCommand(category.Id));

        var entity = await FindAsync<ProductCategory>(category.Id);
        int locPropertyCount = await GetLocalizationStringCountAsync();
        int locValueCount = await GetLocalizationStringValuesCountAsync(); 

        entity.Should().BeNull();
        locPropertyCount.Should().Be(0);
        locValueCount.Should().Be(0);
    }
    #endregion
}