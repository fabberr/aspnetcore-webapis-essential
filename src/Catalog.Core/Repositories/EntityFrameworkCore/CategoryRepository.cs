using Catalog.Core.Abstractions.Repositories;
using Catalog.Core.Models.Entities;
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
public sealed class CategoryRepository(
    DbContext dbContext
)
    : RepositoryBase<Category>(dbContext)
    , ICategoryRepository;
