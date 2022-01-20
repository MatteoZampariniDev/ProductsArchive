using System.Globalization;
using ProductsArchive.Application.Common.Interfaces;

namespace ProductsArchive.WebUI.Services;

/// <summary>
/// Service that intercepts the requested language from a claim in the HttpContext
/// </summary>
public class CurrentCultureService : ICurrentCultureService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentCultureService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string RequestCulture => _httpContextAccessor.HttpContext != null ? 
        _httpContextAccessor.HttpContext.Request.Headers["Request-Culture"] : string.Empty;

    public CultureInfo CurrentCulture => string.IsNullOrWhiteSpace(RequestCulture)
        ? CultureInfo.CurrentCulture
        : CultureInfo.GetCultureInfo(RequestCulture) ?? CultureInfo.CurrentCulture;

    public CultureInfo CurrentUICulture => CultureInfo.CurrentUICulture;
}