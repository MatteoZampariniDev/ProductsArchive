using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductSizes.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductSizes.Commands;

public class DeleteProductSizeCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new DeleteProductSizeCommand(Guid.Empty);

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
        var size = new ProductSize();
        await AddAsync(size);

        var command = new DeleteProductSizeCommand(size.Id);

        // act - assert
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();

        var command = new DeleteProductSizeCommand(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldDeleteProductSizeAsync()
    {
        await RunAsAdministratorAsync();

        var size = new ProductSize();
        await AddAsync(size);

        await SendAsync(new DeleteProductSizeCommand(size.Id));

        var entity = await FindAsync<ProductSize>(size.Id);
        int locPropertyCount = await GetLocalizationStringCountAsync();
        int locValueCount = await GetLocalizationStringValuesCountAsync();

        entity.Should().BeNull();
        locPropertyCount.Should().Be(0);
        locValueCount.Should().Be(0);
    }
    #endregion
}
