using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;
using Catalog.Core.Repositories;

namespace Catalog.Core.Abstractions.Repositories.Generic;

/// <summary>
/// A repository capable of querying data from entities of type
/// <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IQueryableRepository<TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Exposes a <see cref="IQueryable{T}"/> object for querying entities of
    /// type <typeparamref name="TEntity"/> with the configured options.
    /// </summary>
    /// <remarks>
    /// The query enforces a strict ordering by <see cref="EntityBase.Id"/>, in
    /// ascending order.
    /// </remarks>
    /// <param name="options">
    /// Options to use for this query.
    /// </param>
    /// <returns>
    /// An object for querying entities of type <typeparamref name="TEntity"/>.
    /// </returns>
    IQueryable<TEntity> Query(QueryOptions options);

    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="configureOptions">
    /// A delegate for configuring the options to use for this query.<br/>
    /// When not specified, uses <see cref="QueryOptions.Default"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// A collection containing entities of type <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> QueryMultipleAsync(
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/> that meet
    /// the search criteria defined in <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">
    /// Repesents the search criteria as a predicate expression.
    /// </param>
    /// <param name="configureOptions">
    /// A delegate for configuring the options to use for this query.<br/>
    /// When not specified, uses <see cref="QueryOptions.Default"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// A collection containing entities of type <typeparamref name="TEntity"/>
    /// that meet the search criteria.
    /// </returns>
    Task<IEnumerable<TEntity>> QueryMultipleByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds an entity of type <typeparamref name="TEntity"/> by its key.
    /// </summary>
    /// <param name="key">
    /// Key that identifies a specific entity.
    /// </param>
    /// <param name="configureOptions">
    /// A delegate for configuring the options to use for this query.<br/>
    /// When not specified, uses <see cref="QueryOptions.Default"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// The entity, or <see langword="null"/> if no entity with the given
    /// <paramref name="key"/> was found.
    /// </returns>
    Task<TEntity?> FindByIdAsync(
        int key,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds the first entity of type <typeparamref name="TEntity"/>, sorted by
    /// <see cref="EntityBase.Id"/> in ascending order, that meets the search
    /// criteria defined in <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">
    /// Repesents the search criteria as a predicate expression.
    /// </param>
    /// <param name="configureOptions">
    /// A delegate for configuring the options to use for this query.<br/>
    /// When not specified, uses <see cref="QueryOptions.Default"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    /// <returns>
    /// The first entity that meets the search criteria, or
    /// <see langword="null"/> if no matches were found.
    /// </returns>
    Task<TEntity?> FindByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<QueryOptions>? configureOptions = null,
        CancellationToken cancellationToken = default
    );
}
