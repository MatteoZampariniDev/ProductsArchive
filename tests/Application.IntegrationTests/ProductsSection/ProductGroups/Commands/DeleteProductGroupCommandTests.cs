using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductGroups.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductGroups.Commands;

public class DeleteProductGroupCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new DeleteProductGroupCommand(Guid.Empty);

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

        var group = new ProductGroup
        {
            CategoryId = category.Id
        };
        await AddAsync(group);

        var command = new DeleteProductGroupCommand(group.Id);

        // act - assert
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();
        var command = new DeleteProductGroupCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldDeleteProductGroupAsync()
    {
        await RunAsAdministratorAsync();
        var category = new ProductCategory();
        await AddAsync(category);

        var group = new ProductGroup
        {
            CategoryId = category.Id
        };
        await AddAsync(group);

        await SendAsync(new DeleteProductGroupCommand(group.Id));

        var groupEntity = await FindAsync<ProductGroup>(group.Id);
        var categoryEntity = await FindAsync<ProductCategory>(category.Id);

        groupEntity.Should().BeNull();
        categoryEntity.Should().NotBeNull();
    }
    #endregion
}
