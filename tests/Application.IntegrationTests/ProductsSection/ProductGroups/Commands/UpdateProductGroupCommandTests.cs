using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductGroups.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductGroups.Commands;

using static ProductsSectionTesting;
public class UpdateProductGroupCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new UpdateProductGroupCommand();

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
            CategoryId = category.Id,
            GroupId = "00000"
        };
        await AddAsync(group);

        // act - assert
        var command = new UpdateProductGroupCommand
        {
            Id = group.Id,
            CategoryId = category.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();

        // arrange
        var category = new ProductCategory();
        await AddAsync(category);

        // act-assert
        var command = new UpdateProductGroupCommand
        {
            CategoryId = category.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidCategoryIdAsync()
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

        // act-assert
        var command = new UpdateProductGroupCommand
        {
            Id = group.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireNotEmptyGroupIdAsync()
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

        // act-assert
        var command = new UpdateProductGroupCommand
        {
            Id = group.Id,
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

        // arrange
        var category = new ProductCategory();
        await AddAsync(category);

        var group_1 = new ProductGroup
        {
            CategoryId = category.Id,
            GroupId = "00000"
        };
        await AddAsync(group_1);

        var group_2 = new ProductGroup
        {
            CategoryId = category.Id
        };
        await AddAsync(group_2);

        // act-assert
        var command = new UpdateProductGroupCommand
        {
            Id = group_2.Id,
            CategoryId = category.Id,
            GroupId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldUpdateProductGroupAsync()
    {
        // arrange
        var userId = await RunAsAdministratorAsync();
        var culture = RunWithDefaultCulture();

        var category = new ProductCategory();
        await AddAsync(category);

        var group = new ProductGroup
        {
            CategoryId = category.Id
        };
        await AddAsync(group);

        // act
        await SendAsync(new UpdateProductGroupCommand
        {
            Id = group.Id,
            CategoryId = category.Id,
            GroupId = "00000",
            Description = "Updated Description",
            Name = "Updated Name"
        });

        var entity = await GetProductGroupAsync(group.Id);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(group.Id);
        entity.CategoryId.Should().Be(category.Id);
        entity.GroupId.Should().Be("00000");
        entity.Description[culture].Should().Be("Updated Description");
        entity.Name[culture].Should().Be("Updated Name");
        entity.LastModifiedBy.Should().Be(userId);
        entity.LastModified.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
    }
    #endregion
}
