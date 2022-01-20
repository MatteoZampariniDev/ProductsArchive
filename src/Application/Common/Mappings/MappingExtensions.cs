using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ProductsArchive.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync();

    public static IQueryable<TDestination> ProjectToLocalization<TDestination>(this IQueryable queryable, IConfigurationProvider configuration, CultureInfo culture)
        => queryable.ProjectToLocalization<TDestination>(configuration, culture.TwoLetterISOLanguageName);
    public static IQueryable<TDestination> ProjectToLocalization<TDestination>(this IQueryable queryable, IConfigurationProvider configuration, string twoLetterISOLanguageName)
        => queryable.ProjectTo<TDestination>(configuration, new { twoLetterISOLanguageName = twoLetterISOLanguageName });
}
