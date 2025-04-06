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
/// for the <see cref="Product"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="ProductEntityFrameworkCoreRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class ProductEntityFrameworkCoreRepository(CatalogDbContext catalogDbContext)
    :  EntityFrameworkCoreRepositoryBase<Product, int>(catalogDbContext)
    , IProductRepository
{
    #region EntityFrameworkCoreRepositoryBase<Product, int> (implementation)
    public override async Task<IEnumerable<Product>?> GetAsync(uint limit = 10, uint offset = 0, bool includeRelated = false)
    {
        var query = _catalogDbContext.Products
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((int)offset)
            .Take((int)limit);

        return await query.ToArrayAsync();
    }

    public override Task<Product?> GetAsync(int key, bool includeRelated = false)
    {
        return _catalogDbContext.Products
            .FindAsync(key)
            .AsTask();
    }
    #endregion
}
