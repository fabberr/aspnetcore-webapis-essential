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
    protected override DbSet<Category> EntityDbSet => _catalogDbContext.Categories;

    public async Task<IEnumerable<Product>?> GetProducts(int key, uint limit = 10u, uint offset = 0u)
    {
        return await _catalogDbContext.Categories.AsNoTracking()
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
}
