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
    protected override DbSet<Product> EntityDbSet => _catalogDbContext.Products;
}
