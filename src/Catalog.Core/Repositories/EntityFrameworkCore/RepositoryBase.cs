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

    #region Protected Methods
    /// <summary>
    /// Implements <see cref="IQueryableRepository{TEntity}.Query(QueryOptions)"/>.
    /// </summary>
    /// <remarks>
    /// When required internally by other implementations, it's advised to call
    /// this method directly instead of calling the interface method.
    /// </remarks>
    /// <param name="options">
    /// Options to use for this query.
    /// </param>
    /// <param name="additionalPredicates">
    /// A collection of predicated to be added as filters for the query.
    /// </param>
    /// <returns>
    /// An object for querying entities of type <typeparamref name="TEntity"/>.
    /// </returns>
    protected IQueryable<TEntity> QueryImpl(
        QueryOptions options,
        IEnumerable<Expression<Func<TEntity, bool>>>? additionalPredicates = null
    )
    {
        ArgumentNullException.ThrowIfNull(options);

        var query = options.TrackChanges is false
            ? _dbSet.AsNoTracking()
            : _dbSet;

        if (options.IncludeHiddenEntities is false)
        {
            query = query.Where(entity => !entity.Hidden);
        }

        if (additionalPredicates is not null)
        {
            foreach (var predicate in additionalPredicates)
            {
                query = query.Where(predicate);
            }
        }

        return query.OrderBy(entity => entity.Id);
    }

    /// <summary>
    /// Implements <see cref="IQueryableRepository{TEntity}.PaginatedQuery(PaginatedQueryOptions)"/>.
    /// </summary>
    /// <remarks>
    /// When required internally by other implementations, it's advised to call
    /// this method directly instead of calling the interface method.
    /// </remarks>
    /// <param name="options">
    /// Options to use for this query.
    /// </param>
    /// <param name="additionalPredicates">
    /// A collection of predicated to be added as filters for the query.
    /// </param>
    /// <returns>
    /// An object for querying entities of type <typeparamref name="TEntity"/>.
    /// </returns>
    protected IQueryable<TEntity> PaginatedQueryImpl(
        PaginatedQueryOptions options,
        IEnumerable<Expression<Func<TEntity, bool>>>? additionalPredicates = null
    )
    {
        ArgumentNullException.ThrowIfNull(options?.Pagination);

        var query = QueryImpl(
            options: options,
            additionalPredicates: additionalPredicates
        );

        (int pageNumber, int pageSize) = options.Pagination;
        int skipCount = (pageNumber - 1) * pageSize;

        return query.Skip(skipCount).Take(pageSize);
    }
    #endregion

    #region IRepository<TEntity>
    public IQueryable<TEntity> Query(QueryOptions options) => QueryImpl(
        options: options
    );

    public IQueryable<TEntity> PaginatedQuery(PaginatedQueryOptions options) => PaginatedQueryImpl(
        options: options
    );

    public async Task<IEnumerable<TEntity>> QueryMultipleAsync(
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var options = configureOptions?.Invoke() ?? PaginatedQueryOptions.Default;

        var query = options switch {
            PaginatedQueryOptions paginationOptions => PaginatedQueryImpl(options: paginationOptions),
            _ => QueryImpl(options: options),
        };

        return await query.ToArrayAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> QueryMultipleByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var options = configureOptions?.Invoke() ?? PaginatedQueryOptions.Default;

        var query = options switch {
            PaginatedQueryOptions paginationOptions => PaginatedQueryImpl(
                options: paginationOptions,
                additionalPredicates: [predicate]
            ),
            _ => QueryImpl(options: options),
        };

        return await query.ToArrayAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> FindByIdAsync(
        int key,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _dbSet.FindAsync(
            keyValues: [key],
            cancellationToken: cancellationToken
        );

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

        return QueryImpl(options: options)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

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
    #endregion
}
