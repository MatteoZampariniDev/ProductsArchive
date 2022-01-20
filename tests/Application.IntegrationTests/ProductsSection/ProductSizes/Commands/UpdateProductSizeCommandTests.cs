using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductSizes.Commands;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductSizes.Commands;

using static ProductsSectionTesting;

public class UpdateProductSizeCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new UpdateProductSizeCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        var size = new ProductSize();
        await AddAsync(size);

        var command = new UpdateProductSizeCommand
        {
            Id = size.Id
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();

        var command = new UpdateProductSizeCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldUpdateProductSizeAsync()
    {
        var culture = RunWithDefaultCulture();
        var user = await RunAsAdministratorAsync();

        var size = new ProductSize
        {
            Name = LocalizedString.Create(culture, "Size")
        };
        await AddAsync(size);

        await SendAsync(new UpdateProductSizeCommand
        {
            Id = size.Id,
            Name = "Updated"
        });

        var entity = await GetProductSizeAsync(size.Id);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(size.Id);
        entity.Name.Should().NotBeNull();
        entity.Name[culture].Should().Be("Updated");
        entity.LastModifiedBy.Should().Be(user);
        entity.LastModified.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
    }
    #endregion
}
