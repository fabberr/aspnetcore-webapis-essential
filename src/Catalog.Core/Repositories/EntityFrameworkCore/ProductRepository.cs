using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="RepositoryBase{TEntity}"/>
/// for the <see cref="Product"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProductRepository"/> class.
/// </remarks>
/// <param name="dbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance.
/// </param>
public sealed class ProductRepository(
    DbContext dbContext
)
    : RepositoryBase<Product>(dbContext)
    , IProductRepository
{
    public async Task<IEnumerable<Product>> QueryMultipleByCategoryIdAsync(
        int key,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? QueryOptions.Default;

        var query = _dbSet.AsNoTracking()
            .Join(
                inner: _dbContext.Set<Category>().AsNoTracking(),
                outerKeySelector: product => product.CategoryId,
                innerKeySelector: category => category.Id,
                resultSelector: (Product, Category) => new { Product, Category }
            )
            .Where(joined => (
                key == joined.Category.Id &&
                !joined.Product.Hidden &&
                !joined.Category.Hidden
            ))
            .Select(joined => joined.Product);

        if (options.Pagination is not null)
        {
            return query.OrderBy(entity => entity.Id)
                .Skip((options.Pagination.PageNumber - 1) * options.Pagination.PageSize)
                .Take(options.Pagination.PageSize);
        }

        return await query.OrderBy(product => product.Id)
            .ToArrayAsync(cancellationToken: cancellationToken);
    }
}
