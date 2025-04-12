using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Context;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories.Abstractions.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements the <see cref="IRepository{TEntity}"/> interface for Entity
/// Framework Core.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
/// <remarks>
/// Initializes a new instance of the
/// <see cref="RepositoryBase{TEntity}"/> class.
/// </remarks>
/// <param name="catalogDbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance connected to the
/// "Catalog" Database.
/// </param>
public abstract class RepositoryBase<TEntity>(CatalogDbContext catalogDbContext)
    : IRepository<TEntity>
    where TEntity : EntityBase
{
    #region Dependencies
    protected readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    #endregion

    #region Properties
    /// <summary>
    /// Gets a <see cref="DbSet{TEntity}"/> for querying and modifying entities
    /// of type <typeparamref name="TEntity"/>.
    /// </summary>
    protected abstract DbSet<TEntity> DbSet { get; }
    #endregion

    #region IRepository<TEntity>
    public IQueryable<TEntity> Query() => DbSet.AsNoTracking()
        .Where(entity => !entity.Hidden);

    public async Task<IEnumerable<TEntity>> GetAsync(uint limit = 10, uint offset = 0) => await DbSet.AsNoTracking()
        .Where(entity => !entity.Hidden)
        .OrderBy(entity => entity.Id)
        .Skip((int)offset)
        .Take((int)limit)
        .ToArrayAsync();

    public async Task<TEntity?> GetAsync(int key)
    {
        var entity = await DbSet.FindAsync(key);

        if (entity is { Hidden: true })
        {
            return null;
        }

        return entity;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var createdEntityEntry = DbSet.Add(entity);

        await _catalogDbContext.SaveChangesAsync();

        return createdEntityEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var updatedEntityEntry = DbSet.Update(entity);

        await _catalogDbContext.SaveChangesAsync();

        return updatedEntityEntry.Entity;
    }

    public async Task<TEntity?> DeleteAsync(int key, DeleteStrategy strategy = DeleteStrategy.Delete)
    {
        var entity = await DbSet.FindAsync(key);

        if (entity is null)
        {
            return null;
        }

        var deletedEntityEntry = strategy switch {

            DeleteStrategy.Delete => DbSet.Remove(entity),

            DeleteStrategy.Hide => _setAsHiddenAndUpdateEntity(entity),

            _ => throw new NotSupportedException(),
        };

        await _catalogDbContext.SaveChangesAsync();

        return deletedEntityEntry.Entity;

        #region Local Funcions
        EntityEntry<TEntity> _setAsHiddenAndUpdateEntity(TEntity entity) {
            entity.Hidden = true;
            return DbSet.Update(entity);
        }
        #endregion
    }
    #endregion
}
