using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductCategories.Commands;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductCategories.Commands;

using static ProductsSectionTesting;

public class CreateProductCategoryCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new CreateProductCategoryCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();
        var command = new CreateProductCategoryCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldCreateCategoryAsync()
    {
        var userId = await RunAsAdministratorAsync();
        var culture = RunWithDefaultCulture();

        var entityId = await SendAsync(new CreateProductCategoryCommand
        {
            Name = "Category"
        });

        var entity = await GetProductCategoryAsync(entityId);

        entity.Should().NotBeNull();
        entity!.Id.Should().Be(entityId);
        entity.Name.Should().NotBeNull();
        entity.Name[culture].Should().Be("Category");
        entity.CreatedBy.Should().Be(userId);
        entity.Created.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
        entity.LastModifiedBy.Should().Be(null);
        entity.LastModified.Should().Be(null);
    }
    #endregion
}
