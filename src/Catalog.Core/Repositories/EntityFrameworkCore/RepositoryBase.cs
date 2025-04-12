using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
/// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/>
/// class.
/// </remarks>
/// <param name="dbContext">
/// An Entity Framework Core <see cref="DbContext"/> instance.
/// </param>
public abstract class RepositoryBase<TEntity>(DbContext dbContext)
    : IRepository<TEntity>
    where TEntity : EntityBase
{
    #region Dependencies
    protected readonly DbContext _dbContext = dbContext;
    #endregion

    #region Fields
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
    #endregion

    #region IRepository<TEntity>
    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsNoTracking()
            .Where(entity => !entity.Hidden);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        uint limit = 10,
        uint offset = 0,
        CancellationToken cancellationToken = default
    )
    {
        return await Query()
            .OrderBy(entity => entity.Id)
            .Skip((int)offset)
            .Take((int)limit)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(
        int key,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _dbSet.FindAsync([key], cancellationToken);

        if (entity is { Hidden: true })
        {
            return null;
        }

        return entity;
    }

    public async Task<TEntity> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var createdEntityEntry = await _dbSet.AddAsync(entity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return createdEntityEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var updatedEntityEntry = _dbSet.Update(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return updatedEntityEntry.Entity;
    }

    public async Task<TEntity?> DeleteAsync(
        int key,
        DeleteStrategy strategy = DeleteStrategy.Delete,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _dbSet.FindAsync([key], cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var deletedEntityEntry = strategy switch {

            DeleteStrategy.Delete => _dbSet.Remove(entity),

            DeleteStrategy.Hide => _setAsHiddenAndUpdateEntity(entity),

            _ => throw new NotSupportedException(),
        };

        await _dbContext.SaveChangesAsync(cancellationToken);

        return deletedEntityEntry.Entity;

        #region Local Funcions
        EntityEntry<TEntity> _setAsHiddenAndUpdateEntity(TEntity entity) {
            entity.Hidden = true;
            return _dbSet.Update(entity);
        }
        #endregion
    }
    #endregion
}
