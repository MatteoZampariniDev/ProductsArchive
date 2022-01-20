using ProductsArchive.Application.Common.Exceptions;
using ProductsArchive.Application.ProductsSection.ProductGroups.Queries;
using ProductsArchive.Domain.Common.Localization;
using ProductsArchive.Domain.Entities.ProductsSection;

namespace ProductsArchive.Application.IntegrationTests.ProductsSection.ProductGroups.Queries;

public class GetProductGroupQueryTests : TestBase
{
    #region Validation
    [Test]
    public async Task ShouldNotThrowValidationExceptionAsync()
    {
        // arrange
        var category = new ProductCategory();
        await AddAsync(category);

        var group = new ProductGroup
        {
            CategoryId = category.Id
        };
        await AddAsync(group);

        var command = new GetProductGroupQuery(group.Id);

        // act - assert
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidIdAsync()
    {
        var command = new GetProductGroupQuery(Guid.Empty);

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }
    #endregion

    #region Query
    [Test]
    public async Task ShouldReturnProductGroupAsync()
    {
        // arrange
        var culture = RunWithDefaultCulture();

        var category = new ProductCategory();
        await AddAsync(category);

        var group = new ProductGroup
        {
            GroupId = "00000",
            CategoryId = category.Id,
            Name = LocalizedString.Create(culture, "Group"),
            Description = LocalizedString.Create(culture, "Description"),
        };
        await AddAsync(group);

        // act
        var queryResult = await SendAsync(new GetProductGroupQuery(group.Id));

        // assert
        queryResult.Should().NotBeNull();
        queryResult!.Id.Should().Be(group.Id);
        queryResult.Name.Should().Be("Group");
        queryResult.GroupId.Should().Be("00000");
        queryResult.Category.Should().NotBeNull();
        queryResult.Category!.Id.Should().Be(category.Id);
        queryResult.Description.Should().Be("Description");
    }
    #endregion
}
