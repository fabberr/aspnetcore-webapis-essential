using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Abstractions.Repositories.Interfaces;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Abstractions.Repositories;

/// <summary>
/// Implements <see cref="EntityFrameworkCoreRepositoryBase{TEntity, TKey}"/>
/// for the <see cref="Category"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="EntityFrameworkCoreCategoryRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class EntityFrameworkCoreCategoryRepository(CatalogDbContext catalogDbContext)
    :  EntityFrameworkCoreRepositoryBase<Category, int>(catalogDbContext)
    , ICategoryRepository
{
    #region EntityFrameworkCoreRepositoryBase<Category, int> (implementation)
    public override Task<IQueryable<Category>> QueryAsync()
    {
        throw new System.NotImplementedException();
    }

    public override Task<IEnumerable<Category>?> GetAsync(uint limit = 10, uint offset = 0, bool includeRelated = false)
    {
        throw new System.NotImplementedException();
    }

    public override Task<Category?> GetAsync(int key, bool includeRelated = false)
    {
        if (includeRelated is true)
        {
            return _catalogDbContext.Categories.AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == key);
        }

        return _catalogDbContext.Categories.FindAsync(key)
            .AsTask();
    }

    public override Task<Category?> CreateAsync(Category entity)
    {
        throw new System.NotImplementedException();
    }

    public override Task<Category?> DeleteAsync(int key)
    {
        throw new System.NotImplementedException();
    }

    public override Task<Category?> UpdateAsync(int key, Category entity)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region ICategoryRepository
    public Task<IEnumerable<Product>?> GetProducts(int key)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
