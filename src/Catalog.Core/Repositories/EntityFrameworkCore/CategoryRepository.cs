using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Options;
using Catalog.Core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="RepositoryBase{TEntity}"/> for the
/// <see cref="Category"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CategoryRepository"/> class.
/// </remarks>
/// <param name="dbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance.
/// </param>
public sealed class CategoryRepository(DbContext dbContext)
    : RepositoryBase<Category>(dbContext)
    , ICategoryRepository
{
    public async Task<IEnumerable<Product>> QueryMultipleProductsByCategoryIdAsync(
        int key,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? PaginatedQueryOptions.Default;

        var query = _dbSet.AsNoTracking()
            .Join(
                _dbContext.Set<Product>().AsNoTracking(),
                category => category.Id, product => product.CategoryId,
                (category, product) => new { Category = category, Product = product }
            )
            .Where(categoryProduct => (
                categoryProduct.Category.Id == key
                && !categoryProduct.Category.Hidden
                && !categoryProduct.Product.Hidden
            ))
            .Select(categoryProduct => categoryProduct.Product)
            .OrderBy(product => product.Id);

        if (options is PaginatedQueryOptions paginationOptions and { Limit: > 0 })
        {
            var (_, limit, offset) = paginationOptions;
            query = query.Skip(offset).Take(limit)
                as IOrderedQueryable<Product>;
        }

        return await query!.ToArrayAsync(cancellationToken);
    }
}
