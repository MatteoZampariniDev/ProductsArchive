using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.Products.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.Products.Commands;

public class CreateProductCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new CreateProductCommand();

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
        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        var command = new CreateProductCommand
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidGroupIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var size = new ProductSize();
        await AddAsync(size);

        var command = new CreateProductCommand
        {
            SizeId = size.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidSizeIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        // act-assert
        var command = new CreateProductCommand
        {
            GroupId = group.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireNotEmptyProductIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act-assert
        var command = new CreateProductCommand
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = ""
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueProductIdAsync()
    {
        await RunAsAdministratorAsync();

        //arrange
        var product = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize(),
            ProductId = "00000"
        };
        await AddAsync(product);

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act-assert
        var command = new CreateProductCommand
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Command
    [Test]
    public async Task ShouldCreateProductAsync()
    {
        //arrange
        var userId = await RunAsAdministratorAsync();

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act
        var entityId = await SendAsync(new CreateProductCommand
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        });

        var entity = await FindAsync<Product>(entityId);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(entityId);
        entity.GroupId.Should().Be(group.Id);
        entity.SizeId.Should().Be(size.Id);
        entity.ProductId.Should().Be("00000");
        entity.CreatedBy.Should().Be(userId);
        entity.Created.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
        entity.LastModifiedBy.Should().Be(null);
        entity.LastModified.Should().Be(null);
    }
    #endregion
}