using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

public abstract partial class RepositoryBase<TEntity>
    : IRepository<TEntity>
    where TEntity : EntityBase
{
    public async Task<TEntity> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var createdEntityEntry = await _dbSet.AddAsync(
            entity: entity,
            cancellationToken: cancellationToken
        );

        return createdEntityEntry.Entity;
    }

    public Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var updatedEntityEntry = _dbSet.Update(entity);

        return Task.FromResult(updatedEntityEntry.Entity);
    }

    public async Task<TEntity?> RemoveByIdAsync(
        int key,
        RemoveStrategy strategy = RemoveStrategy.Delete,
        CancellationToken cancellationToken = default
    )
    {
        var currentEntity = await FindByIdAsync(
            key: key,
            cancellationToken: cancellationToken
        );

        if (currentEntity is null)
        {
            return null;
        }

        var removedEntityEntry = strategy switch {
            RemoveStrategy.Delete
                => _dbSet.Remove(currentEntity),

            RemoveStrategy.Hide
                => _setAsHiddenAndUpdate(currentEntity),

            _ => throw new NotSupportedException(),
        };

        return removedEntityEntry.Entity;

        #region Local Functions
        EntityEntry<TEntity> _setAsHiddenAndUpdate(TEntity entity)
        {
            entity.Hidden = true;
            return _dbSet.Update(entity);
        }
        #endregion
    }

    public async Task<IEnumerable<TEntity>> RemoveByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        RemoveStrategy strategy = RemoveStrategy.Delete,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var entities = await QueryImpl(
            options: new QueryOptions(TrackChanges: true),
            additionalPredicates: [predicate]
        ).ToArrayAsync(cancellationToken: cancellationToken);

        switch (strategy)
        {
            case RemoveStrategy.Delete:
                _dbSet.RemoveRange(entities);
                break;
            case RemoveStrategy.Hide:
                _setAsHiddenAndUpdateRange(entities);
                break;
            default:
                throw new NotSupportedException();
        }

        return entities;

        #region Local Functions
        void _setAsHiddenAndUpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Hidden = true;
            }
            _dbSet.UpdateRange(entities);
        }
        #endregion
    }
}
