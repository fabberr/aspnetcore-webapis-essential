using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories;

/// <summary>
/// Implements <see cref="EntityFrameworkCoreRepositoryBase{TEntity}"/>
/// for the <see cref="Product"/> entity.
/// </summary>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="EntityFrameworkCoreProductRepository"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public sealed class EntityFrameworkCoreProductRepository(CatalogDbContext catalogDbContext)
    :  EntityFrameworkCoreRepositoryBase<Product>(catalogDbContext)
    , IProductRepository
{
    protected override DbSet<Product> DbSet => _catalogDbContext.Products;
}
