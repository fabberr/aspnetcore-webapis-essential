using System;
using System.Linq;
using Catalog.Core.Abstractions.Pagination;
using Catalog.Core.Pagination;

namespace Catalog.Core.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Gets the next <paramref name="pageSize"/> items in the given
    /// <paramref name="pageNumber"/> from the data source.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the data in the data source.
    /// </typeparam>
    /// <param name="source">
    /// Data source.
    /// </param>
    /// <param name="pageNumber">
    /// The page number.
    /// </param>
    /// <param name="pageSize">
    /// Number of items to fetch from the data source.
    /// </param>
    /// <returns></returns>
    public static IPaginatedCollection<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        int skipCount = (pageNumber - 1) * pageSize;
        return new PaginatedCollection<T>(
            items: source.Skip(skipCount).Take(pageSize),
            totalCount: source.Count(),
            pageNumber: pageNumber,
            pageSize: pageSize
        );
    }
}
