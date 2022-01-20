using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.Products.Commands;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.Products.Commands;

public class UpdateProductCommandTests : TestBase
{
    #region Authorization
    [Test]
    public async Task ShouldThrowForbiddenAccessExceptionAsync()
    {
        await RunAsDefaultUserAsync();
        var command = new UpdateProductCommand();

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

        // act - assert
        var command = new UpdateProductCommand
        {
            Id = product.Id,
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        await RunAsAdministratorAsync();

        // arrange
        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act - assert
        var command = new UpdateProductCommand
        {
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidProductIdAsync()
    {
        await RunAsAdministratorAsync();

        // arrange
        var product = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(product);

        var size = new ProductSize();
        await AddAsync(size);

        // act - assert
        var command = new UpdateProductCommand
        {
            Id = product.Id,
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

        // arrange
        var product = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product);

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        // act - assert
        var command = new UpdateProductCommand
        {
            Id = product.Id,
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

        // arrange
        var product = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product);

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act - assert
        var command = new UpdateProductCommand
        {
            Id = product.Id,
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

        // arrange
        var product_1 = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product_1);

        var product_2 = new Product
        {
            ProductId = "00000",
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product_2);

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act - assert
        var command = new UpdateProductCommand
        {
            Id = product_1.Id,
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
    public async Task ShouldUpdateProductAsync()
    {
        // arrange
        var user = await RunAsAdministratorAsync();

        var product = new Product
        {
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };
        await AddAsync(product);

        var group = new ProductGroup
        {
            Category = new ProductCategory()
        };
        await AddAsync(group);

        var size = new ProductSize();
        await AddAsync(size);

        // act
        await SendAsync(new UpdateProductCommand
        {
            Id = product.Id,
            GroupId = group.Id,
            SizeId = size.Id,
            ProductId = "00000"
        });

        var entity = await FindAsync<Product>(product.Id);

        // assert
        entity.Should().NotBeNull();
        entity!.Id.Should().Be(product.Id);
        entity.GroupId.Should().Be(group.Id);
        entity.SizeId.Should().Be(size.Id);
        entity.ProductId.Should().Be("00000");
        entity.LastModifiedBy.Should().Be(user);
        entity.LastModified.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
    }
    #endregion
}
