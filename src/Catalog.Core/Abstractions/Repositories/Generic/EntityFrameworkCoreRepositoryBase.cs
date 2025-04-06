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

    #region Properties
    /// <summary>
    /// Gets a <see cref="DbSet{TEntity}"/> for querying entities of type
    /// <typeparamref name="TEntity"/>.
    /// </summary>
    protected abstract DbSet<TEntity> EntityDbSet { get; }
    #endregion

    #region IRepository<TEntity, TKey>
    public async Task<IEnumerable<TEntity>?> GetAsync(uint limit = 10, uint offset = 0)
    {
        return await EntityDbSet.AsNoTracking()
            .Where(entity => !entity.Hidden)
            .OrderBy(entity => entity.Id)
            .Skip((int)offset)
            .Take((int)limit)
            .ToArrayAsync();
    }

    public async Task<TEntity?> GetAsync(TKey key)
    {
        var entity = await EntityDbSet.FindAsync(key);
        if (entity is { Hidden: true })
        {
            return null;
        }

        return entity;
    }

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
