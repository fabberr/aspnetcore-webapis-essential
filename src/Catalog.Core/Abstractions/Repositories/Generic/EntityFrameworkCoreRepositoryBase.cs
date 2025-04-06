using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic.Interfaces;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Abstractions.Repositories.Generic;

/// <summary>
/// Implements the <see cref="IRepository{TEntity, TKey}"/> interface for Entity
/// Framework Core.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
/// <typeparam name="TKey">
/// Type of the key used to identify the entity.
/// </typeparam>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="EntityFrameworkCoreRepositoryBase{TEntity, TKey}"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public abstract class EntityFrameworkCoreRepositoryBase<TEntity, TKey>(CatalogDbContext catalogDbContext)
    : IRepository<TEntity, TKey>
    where TEntity : EntityBase
    where TKey : notnull
{
    #region Dependencies
    protected readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    #endregion

    #region IRepository<TEntity, TKey> (abstract)
    public abstract Task<IEnumerable<TEntity>?> GetAsync(uint limit = 10, uint offset = 0, bool includeRelated = false);

    public abstract Task<TEntity?> GetAsync(TKey key, bool includeRelated = false);

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var createdEntry = await _catalogDbContext.AddAsync(entity);

        await _catalogDbContext.SaveChangesAsync();

        return createdEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _catalogDbContext.Entry(entity).State = EntityState.Modified;

        await _catalogDbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        _catalogDbContext.Remove(entity);

        await _catalogDbContext.SaveChangesAsync();

        return entity;
    }
    #endregion
}
