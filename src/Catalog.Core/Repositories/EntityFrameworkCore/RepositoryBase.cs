using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Enums;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Options;
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
    public IQueryable<TEntity> Query(QueryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var query = options.TrackChanges ? _dbSet : _dbSet.AsNoTracking();

        if (options.IncludeHiddenEntities is false)
        {
            query = query.Where(entity => !entity.Hidden);
        }
        
        query = query.OrderBy(entity => entity.Id);

        if (options is PaginatedQueryOptions paginationOptions and { Limit: > 0 })
        {
            query = query.Skip(paginationOptions.Offset).Take(paginationOptions.Limit)
                as IOrderedQueryable<TEntity>;
        }

        return query!;
    }

    public async Task<IEnumerable<TEntity>> QueryMultipleAsync(
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? PaginatedQueryOptions.Default;

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

        var options = configureOptions?.Invoke() ?? PaginatedQueryOptions.Default;

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

        var removedEntity = strategy switch {
            RemoveStrategy.Delete
                => _dbSet.Remove(currentEntity),

            RemoveStrategy.Hide
                => _setAsHiddenAndUpdate(currentEntity),

            _ => throw new NotSupportedException(),
        };

        await _dbContext.SaveChangesAsync(cancellationToken);

        return removedEntity.Entity;

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

        var options = new PaginatedQueryOptions(
            TrackChanges: true,
            Limit: -1
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

        await _dbContext.SaveChangesAsync(cancellationToken);

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
