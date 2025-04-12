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
/// <param name="dbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance.
/// </param>
public sealed class ProductRepository(DbContext dbContext) : RepositoryBase<Product>(dbContext), IProductRepository;
