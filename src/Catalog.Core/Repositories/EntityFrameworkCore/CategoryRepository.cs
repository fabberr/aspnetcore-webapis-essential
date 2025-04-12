using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="RepositoryBase{TEntity}"/>
/// for the <see cref="Category"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CategoryRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class CategoryRepository(CatalogDbContext catalogDbContext)
    : RepositoryBase<Category>(catalogDbContext)
    , ICategoryRepository
{
    protected override DbSet<Category> DbSet => _catalogDbContext.Categories;

    public async Task<IEnumerable<Product>> GetProducts(int key, uint limit = 10u, uint offset = 0u)
    {
        return await _catalogDbContext.Categories.AsNoTracking()
            .Join(
                _catalogDbContext.Products.AsNoTracking(),
                (category) => category.Id, (product) => product.CategoryId,
                (category, product) => new { category, product }
            )
            .Where((categoryProduct) => 
                categoryProduct.category.Id == key
                && !categoryProduct.category.Hidden
                && !categoryProduct.product.Hidden
            )
            .Select(categoryProduct => categoryProduct.product)
            .OrderBy(p => p.Id)
            .Skip((int)offset)
            .Take((int)limit)
            .ToArrayAsync();
    }
}
