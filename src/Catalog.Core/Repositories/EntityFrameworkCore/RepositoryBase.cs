using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;
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
    public IQueryable<TEntity> Query(QueryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var query = options.TrackChanges is false
            ? _dbSet.AsNoTracking()
            : _dbSet;

        if (options.IncludeHiddenEntities is false)
        {
            query = query.Where(entity => !entity.Hidden);
        }
        
        if (options.Pagination is not null)
        {
            return query.OrderBy(entity => entity.Id)
                .Skip((options.Pagination.PageNumber - 1) * options.Pagination.PageSize)
                .Take(options.Pagination.PageSize);
        }

        return query.OrderBy(entity => entity.Id);
    }

    public async Task<IEnumerable<TEntity>> QueryMultipleAsync(
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? QueryOptions.Default;

        return await Query(options: options)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> QueryMultipleByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var options = configureOptions?.Invoke() ?? QueryOptions.Default;

        return await Query(options: options)
            .Where(predicate)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<TEntity?> FindByIdAsync(
        int key,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _dbSet.FindAsync([key], cancellationToken);

        var options = configureOptions?.Invoke() ?? QueryOptions.Default;

        return entity switch {
            { Hidden: true } when options.IncludeHiddenEntities is false => null,
            _ => entity
        };
    }

    public Task<TEntity?> FindByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? QueryOptions.Default;

        return Query(options: options)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entity);

        var createdEntityEntry = await _dbSet.AddAsync(entity, cancellationToken);

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

        var options = new QueryOptions(
            Pagination: null,
            TrackChanges: true
        );

        var entities = await Query(options: options)
            .Where(predicate)
            .ToArrayAsync(cancellationToken);

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
    #endregion
}
