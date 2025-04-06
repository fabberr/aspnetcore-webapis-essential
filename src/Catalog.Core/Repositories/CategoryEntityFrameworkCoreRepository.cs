using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories;

/// <summary>
/// Implements <see cref="EntityFrameworkCoreRepositoryBase{TEntity, TKey}"/>
/// for the <see cref="Category"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="CategoryEntityFrameworkCoreRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class CategoryEntityFrameworkCoreRepository(CatalogDbContext catalogDbContext)
    :  EntityFrameworkCoreRepositoryBase<Category, int>(catalogDbContext)
    , ICategoryRepository
{
    #region EntityFrameworkCoreRepositoryBase<Category, int> (implementation)
    public override async Task<IEnumerable<Category>?> GetAsync(uint limit = 10, uint offset = 0, bool includeRelated = false)
    {
        var query = _catalogDbContext.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((int)offset)
            .Take((int)limit);

        if (includeRelated)
        {
            query = query.Include(c => c.Products);
        }

        return await query.ToArrayAsync();
    }

    public override Task<Category?> GetAsync(int key, bool includeRelated = false)
    {
        if (includeRelated is true)
        {
            return _catalogDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == key);
        }

        return _catalogDbContext.Categories
            .FindAsync(key)
            .AsTask();
    }
    #endregion

    #region ICategoryRepository
    public async Task<IEnumerable<Product>?> GetProducts(int key, uint limit = 10u, uint offset = 0u)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id == key)
            .Join(
                _catalogDbContext.Products.AsNoTracking(),
                category => category.Id, product => product.CategoryId,
                (_, product) => product
            )
            .OrderBy(p => p.Id)
            .Skip((int)offset)
            .Take((int)limit)
            .ToArrayAsync();
    }
    #endregion
}
