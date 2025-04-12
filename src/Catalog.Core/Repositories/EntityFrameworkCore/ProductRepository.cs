using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="RepositoryBase{TEntity}"/>
/// for the <see cref="Product"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProductRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class ProductRepository(CatalogDbContext catalogDbContext)
    :  RepositoryBase<Product>(catalogDbContext)
    , IProductRepository
{
    protected override DbSet<Product> DbSet => _catalogDbContext.Products;
}
