using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
/// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/>
/// class.
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
    protected abstract DbSet<TEntity> EntityDbSet { get; }
    #endregion

    #region IRepository<TEntity>
    public IQueryable<TEntity> Query()
    {
        return EntityDbSet.AsNoTracking()
            .Where(entity => !entity.Hidden);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(
        uint limit = 10,
        uint offset = 0,
        CancellationToken cancellationToken = default
    )
    {
        return await EntityDbSet.AsNoTracking()
            .Where(entity => !entity.Hidden)
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
        var entity = await EntityDbSet.FindAsync([key], cancellationToken);

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

        var createdEntityEntry = await EntityDbSet.AddAsync(entity, cancellationToken);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return createdEntityEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var updatedEntityEntry = EntityDbSet.Update(entity);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return updatedEntityEntry.Entity;
    }

    public async Task<TEntity?> DeleteAsync(
        int key,
        DeleteStrategy strategy = DeleteStrategy.Delete,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await EntityDbSet.FindAsync([key], cancellationToken);

        if (entity is null)
        {
            return null;
        }

        var deletedEntityEntry = strategy switch {

            DeleteStrategy.Delete => EntityDbSet.Remove(entity),

            DeleteStrategy.Hide => _setAsHiddenAndUpdateEntity(entity),

            _ => throw new NotSupportedException(),
        };

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return deletedEntityEntry.Entity;

        #region Local Funcions
        EntityEntry<TEntity> _setAsHiddenAndUpdateEntity(TEntity entity) {
            entity.Hidden = true;
            return EntityDbSet.Update(entity);
        }
        #endregion
    }
    #endregion
}
