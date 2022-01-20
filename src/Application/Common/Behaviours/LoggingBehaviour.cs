using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace ProductsArchive.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentCultureService _currentCultureService;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, ICurrentCultureService currentCultureService, IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
        _currentCultureService = currentCultureService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;
        string userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await _identityService.GetUserNameAsync(userId);
        }

        _logger.LogInformation("\n" +
            $"Request: {requestName}\n" +
            $"UserId: {userId}\n" +
            $"UserName: {userName}\n" +
            $"Culture: {_currentCultureService.CurrentCulture}\n" +
            $"UICulture: {_currentCultureService.CurrentUICulture}\n" +
            $"Details: {request.ToJson()}");
    }
}
