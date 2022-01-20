using ProductsArchive.Application.Common.Behaviours;
using ProductsArchive.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProductsArchive.Application.ProductsSection.ProductCategories.Commands;

namespace ProductsArchive.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateProductCategoryCommand>> _logger = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;
    private Mock<IIdentityService> _identityService = null!;
    private Mock<ICurrentCultureService> _cultureService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateProductCategoryCommand>>();
        _currentUserService = new Mock<ICurrentUserService>();
        _identityService = new Mock<IIdentityService>();
        _cultureService = new Mock<ICurrentCultureService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _currentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateProductCategoryCommand>(_logger.Object, _currentUserService.Object, _cultureService.Object, _identityService.Object);

        await requestLogger.Process(new CreateProductCategoryCommand { Name = "Category" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateProductCategoryCommand>(_logger.Object, _currentUserService.Object, _cultureService.Object, _identityService.Object);

        await requestLogger.Process(new CreateProductCategoryCommand { Name = "Category" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
