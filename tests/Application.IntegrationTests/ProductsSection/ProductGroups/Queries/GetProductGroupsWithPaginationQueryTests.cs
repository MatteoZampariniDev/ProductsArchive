﻿using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.Common.Models;
using ProductsArchive.Application.ProductsSection.ProductGroups.Queries;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductGroups.Queries;

public class GetProductGroupsWithPaginationQueryTests : TestBase
{
    #region Validation
    [Test]
    public async Task ShouldBeValidRequestAsync()
    {
        var command = new GetProductGroupsWithPaginationQuery();

        await FluentActions.Invoking(() => SendAsync(command))
                                    .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidPageNumberAsync()
    {
        var command = new GetProductGroupsWithPaginationQuery
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
        var command = new GetProductGroupsWithPaginationQuery
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
        var command = new GetProductGroupsWithPaginationQuery
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
        var command = new GetProductGroupsWithPaginationQuery
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
        var query = await SendAsync(new GetProductGroupsWithPaginationQuery());

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
    public async Task ShouldReturnProductGroupsWithPaginationFilterSortingAsync()
    {
        var culture = RunWithDefaultCulture();

        var category = new ProductCategory();
        await AddAsync(category);

        var group_1 = new ProductGroup
        {
            CategoryId = category.Id,
            Name = LocalizedString.Create(culture, "AA")
        };
        var group_2 = new ProductGroup
        {
            CategoryId = category.Id,
            Name = LocalizedString.Create(culture, "AB")
        };
        var group_3 = new ProductGroup
        {
            CategoryId = category.Id,
            Name = LocalizedString.Create(culture, "AC")
        };
        var group_4 = new ProductGroup
        {
            CategoryId = category.Id,
            Name = LocalizedString.Create(culture, "D")
        };

        await AddAsync(group_1);
        await AddAsync(group_2);
        await AddAsync(group_3);
        await AddAsync(group_4);

        var query_1 = await SendAsync(new GetProductGroupsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageIndex = 0,
                PageSize = 1
            },
            SortDetails = new SortDetails
            {
                Order = "DESC",
                Property = "name"
            },
            FilterDetails = new List<FilterDetails>
            {
                new FilterDetails
                {
                    Property = "name",
                    Query = "a"
                }
            }
        });

        var query_2 = await SendAsync(new GetProductGroupsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageIndex = 2,
                PageSize = 1
            },
            SortDetails = new SortDetails
            {
                Order = "ASC",
                Property = "name"
            },
            FilterDetails = new List<FilterDetails>
            {
                new FilterDetails
                {
                    Property = "name",
                    Query = "a"
                }
            }
        });

        var query_3 = await SendAsync(new GetProductGroupsWithPaginationQuery
        {
            PaginationDetails = new PaginationDetails
            {
                PageIndex = 0,
                PageSize = 1
            },
            SortDetails = new SortDetails
            {
                Order = "ASC",
                Property = "name"
            },
            FilterDetails = new List<FilterDetails>
            {
                new FilterDetails
                {
                    Property = "name",
                    Query = "d"
                }
            }
        });

        // assert
        query_1.Should().NotBeNull();
        query_1.PageIndex.Should().Be(0);
        query_1.Items.Count.Should().Be(1);
        query_1.TotalCount.Should().Be(3);
        query_1.TotalPages.Should().Be(3);
        query_1.HasNextPage.Should().BeTrue();
        query_1.HasPreviousPage.Should().BeFalse();
        query_1.Items[0].Name.Should().Be("AC");

        query_2.Should().NotBeNull();
        query_2.PageIndex.Should().Be(2);
        query_2.Items.Count.Should().Be(1);
        query_2.TotalCount.Should().Be(3);
        query_2.TotalPages.Should().Be(3);
        query_2.HasNextPage.Should().BeFalse();
        query_2.HasPreviousPage.Should().BeTrue();
        query_2.Items[0].Name.Should().Be("AC");

        query_3.Should().NotBeNull();
        query_3.PageIndex.Should().Be(0);
        query_3.Items.Count.Should().Be(1);
        query_3.TotalCount.Should().Be(1);
        query_3.TotalPages.Should().Be(1);
        query_3.HasNextPage.Should().BeFalse();
        query_3.HasPreviousPage.Should().BeFalse();
        query_3.Items[0].Name.Should().Be("D");
    }
    #endregion
}
