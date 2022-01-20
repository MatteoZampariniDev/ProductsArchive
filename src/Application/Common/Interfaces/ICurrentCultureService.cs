using System.Globalization;

namespace ProductsArchive.Application.Common.Interfaces;

public interface ICurrentCultureService
{
    CultureInfo CurrentCulture { get; }
    CultureInfo CurrentUICulture { get; }
}
