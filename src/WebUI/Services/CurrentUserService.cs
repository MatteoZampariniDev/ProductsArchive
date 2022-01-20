using System.Security.Claims;
using ProductsArchive.Application.Common.Interfaces;

namespace ProductsArchive.WebUI.Services;

/// <summary>
/// Service that intercepts the userId from the a claim in the HttpContext
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
