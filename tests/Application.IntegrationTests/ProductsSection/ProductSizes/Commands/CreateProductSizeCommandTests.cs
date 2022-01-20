using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductSizes.Commands;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductSizes.Commands;

using static ProductsSectionTesting;

public class CreateProductSizeCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new CreateProductSizeCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        var command = new CreateProductSizeCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldCreateProductSizeAsync()
    {
        // arrange
        var culture = RunWithDefaultCulture();
        var userId = await RunAsAdministratorAsync();

        // act
        var entityId = await SendAsync(new CreateProductSizeCommand
        {
            Name = "Size"
        });

        var entity = await GetProductSizeAsync(entityId);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(entityId);
        entity.Name.Should().NotBeNull();
        entity.Name[culture].Should().Be("Size");
        entity.CreatedBy.Should().Be(userId);
        entity.Created.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
        entity.LastModifiedBy.Should().Be(null);
        entity.LastModified.Should().Be(null);
    }
    #endregion
}
