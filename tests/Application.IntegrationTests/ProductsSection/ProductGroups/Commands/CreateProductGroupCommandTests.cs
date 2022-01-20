using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductGroups.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductGroups.Commands;

using static ProductsSectionTesting;

public class CreateProductGroupCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new CreateProductGroupCommand();

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }
    #endregion

    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var category = new ProductCategory();
        await AddAsync(category);

        // act-assert
        var command = new CreateProductGroupCommand
        {
            CategoryId = category.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidCategoryAsync()
    {
        await RunAsAdministratorAsync();
        var command = new CreateProductGroupCommand
        {
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireNotEmptyGroupIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var category = new ProductCategory();
        await AddAsync(category);

        // act-assert
        var command = new CreateProductGroupCommand
        {
            CategoryId = category.Id,
            GroupId = ""
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueGroupIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var category = new ProductCategory();
        await AddAsync(category);

        await AddAsync(new ProductGroup
        {
            CategoryId = category.Id,
            GroupId = "00000"
        });

        // act-assert
        var command = new CreateProductGroupCommand
        {
            CategoryId = category.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldCreateProductGroupAsync()
    {
        // arrange
        var userId = await RunAsAdministratorAsync();
        var culture = RunWithDefaultCulture();

        var category = new ProductCategory();
        await AddAsync(category);

        // act
        var entityId = await SendAsync(new CreateProductGroupCommand
        {
            CategoryId = category.Id,
            GroupId = "00000",
            Description = "Description",
            Name = "Name"
        });

        var entity = await GetProductGroupAsync(entityId);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(entityId);
        entity.GroupId.Should().Be("00000");
        entity.CategoryId.Should().Be(category.Id);
        entity.Description[culture].Should().Be("Description");
        entity.Name[culture].Should().Be("Name");
        entity.CreatedBy.Should().Be(userId);
        entity.Created.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
        entity.LastModifiedBy.Should().Be(null);
        entity.LastModified.Should().Be(null);
    }
    #endregion
}