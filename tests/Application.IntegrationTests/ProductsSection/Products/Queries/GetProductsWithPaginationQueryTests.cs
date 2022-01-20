using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection.Products.Queries;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.Products.Queries;

public class GetProductsWithPaginationQueryTests : TestBase
{
    #region Validation
    [Test]
    public async Task ShouldBeValidRequestAsync()
    {
        var command = new GetProductsWithPaginationQuery();

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidPageNumberAsync()
    {
        var command = new GetProductsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageIndex = -1
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidPageSizeAsync()
    {
        var command = new GetProductsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageSize = -1
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidSortOrderAsync()
    {
        var command = new GetProductsWithPaginationQuery
        {
            SortDetails = new SortDetails
            {
                Order = "random"
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidSortColumnAsync()
    {
        var command = new GetProductsWithPaginationQuery
        {
            SortDetails = new SortDetails
            {
                Property = "NotExistingProperty"
            }
        };

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Query
    [Test]
    public async Task ShouldReturnEmptyPaginationAsync()
    {
        // act
        var query = await SendAsync(new GetProductsWithPaginationQuery());

        // assert
        query.Should().NotBeNull();
        query.PageIndex.Should().Be(0);
        query.Items.Count.Should().Be(0);
        query.TotalPages.Should().Be(0);
        query.TotalCount.Should().Be(0);
        query.HasNextPage.Should().BeFalse();
        query.HasPreviousPage.Should().BeFalse();
        query.Items.Count.Should().Be(0);
    }

    [Test]
    public async Task ShouldReturnProductsWithPaginationFilterSortingAsync()
    {
        // arrange
        var product_1 = new Product
        {
            ProductId = "01",
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };

        var product_2 = new Product
        {
            ProductId = "02",
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };

        var product_3 = new Product
        {
            ProductId = "03",
            Group = new ProductGroup
            {
                Category = new ProductCategory()
            },
            Size = new ProductSize()
        };

        await AddAsync(product_1);
        await AddAsync(product_2);
        await AddAsync(product_3);

        // act
        var queryResult = await SendAsync(new GetProductsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageIndex = 0,
                PageSize = 2
            }
        });

        // assert
        queryResult.Should().NotBeNull();
        queryResult.PageIndex.Should().Be(0);
        queryResult.Items.Count.Should().Be(2);
        queryResult.TotalCount.Should().Be(3);
        queryResult.TotalPages.Should().Be(2);
        queryResult.HasNextPage.Should().BeTrue();
        queryResult.HasPreviousPage.Should().BeFalse();
    }
    #endregion
}
