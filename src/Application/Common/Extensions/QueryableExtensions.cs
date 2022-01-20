using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProductsArchive.Application.Common.Models;

namespace ProductsArchive.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> IncludeLocalizedProperty<TEntity>([NotNull] this IQueryable<TEntity> source, [NotNull] Expression<Func<TEntity, LocalizedString>> navigationPropertyPath)
    where TEntity : class
    {
        return source.Include(navigationPropertyPath).ThenInclude(x => x.LocalizedValues);
    }

    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, PaginationDetails paginationDetails, CancellationToken cancellationToken)
        => queryable.ToPaginatedListAsync(paginationDetails.PageIndex, paginationDetails.PageSize, cancellationToken);
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageIndex, int pageSize, CancellationToken cancellationToken)
        => PaginatedList<TDestination>.CreateAsync(queryable, pageIndex, pageSize, cancellationToken);

    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, SortDetails sortDetails)
    {
        if (sortDetails != null
            && !string.IsNullOrWhiteSpace(sortDetails.Property)
            && TypeHelpers.IsValidProperty<TSource>(sortDetails.Property))
        {
            string order = !string.IsNullOrWhiteSpace(sortDetails.Order) && sortDetails.Order.ToUpper() == "ASC" ? "ASC" : "DESC";

            source = source.OrderBy(string.Format("{0} {1}", sortDetails.Property, order));
        }

        return source;
    }

    private static bool IsValidFilter<TSource>(FilterDetails filter)
        => filter != null && !string.IsNullOrWhiteSpace(filter.Property) && !string.IsNullOrWhiteSpace(filter.Query) && TypeHelpers.IsValidProperty<TSource>(filter.Property);

    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, params FilterDetails[] filters)
    {
        string dynamicQuery = "";
        var queries = new List<string>();

        foreach (var f in filters)
        {
            if (IsValidFilter<TSource>(f))
            {
                if (queries.Count == 0)
                {
                    dynamicQuery = string.Format("{0}.ToLower().Contains(@" + queries.Count + ")", f.Property);
                }
                else
                {
                    dynamicQuery += string.Format(" || {0}.ToLower().Contains(@" + queries.Count + ")", f.Property);
                }

                queries.Add(f.Query!.ToLower());
            }
        }

        if (!string.IsNullOrWhiteSpace(dynamicQuery))
        {
            source = source.Where(dynamicQuery, queries.ToArray());
        }

        return source;
    }
}
