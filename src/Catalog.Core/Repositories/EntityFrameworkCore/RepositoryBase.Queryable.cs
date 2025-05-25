using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories.Generic;
using Catalog.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

public abstract partial class RepositoryBase<TEntity>
    : IRepository<TEntity>
    where TEntity : EntityBase
{
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

    #region Implementation Details
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
    private IQueryable<TEntity> QueryImpl(
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
    private IQueryable<TEntity> PaginatedQueryImpl(
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
}
