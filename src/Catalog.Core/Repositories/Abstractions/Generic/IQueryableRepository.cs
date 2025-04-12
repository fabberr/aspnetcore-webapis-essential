using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Repositories.Abstractions.Generic;

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
    /// type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Note: Entities marked as hidden (<see cref="EntityBase.Hidden"/> is set
    /// to <see langword="true"/>) will <b>not</b> be fetched from the data
    /// source when using this method.
    /// </remarks>
    /// <returns>
    /// A <see cref="IQueryable{T}"/> instance.
    /// </returns>
    IQueryable<TEntity> Query();

    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/>, sorted by
    /// <see cref="EntityBase.Id"/> in ascending order.
    /// </summary>
    /// <remarks>
    /// Note: Entities marked as hidden (<see cref="EntityBase.Hidden"/> is set
    /// to <see langword="true"/>) will <b>not</b> be fetched from the data
    /// source when using this method.
    /// </remarks>
    /// <param name="limit">
    /// Delimits the number of entries which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entries to skip.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// A collection containing entities of type <typeparamref name="TEntity"/>.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAsync(
        uint limit = 10u,
        uint offset = 0u,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Queries for entities of type <typeparamref name="TEntity"/> that meet
    /// the search criteria defined in <paramref name="predicate"/>, sorted by
    /// <see cref="EntityBase.Id"/> in ascending order.
    /// </summary>
    /// <remarks>
    /// Note: Entities marked as hidden (<see cref="EntityBase.Hidden"/> is set
    /// to <see langword="true"/>) will <b>not</b> be fetched from the data
    /// source when using this method.
    /// </remarks>
    /// <param name="predicate">
    /// Repesents the search criteria as a predicate expression.
    /// </param>
    /// <param name="limit">
    /// Delimits the number of entries which will be fetched at most.
    /// </param>
    /// <param name="offset">
    /// Number of entries to skip.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// A collection containing entities of type <typeparamref name="TEntity"/>
    /// that meet the search criteria.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        uint limit = 10u,
        uint offset = 0u,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds an entity of type <typeparamref name="TEntity"/> by its key.
    /// </summary>
    /// <remarks>
    /// Note: Entities marked as hidden (<see cref="EntityBase.Hidden"/> is set
    /// to <see langword="true"/>) will <b>not</b> be fetched from the data
    /// source when using this method.
    /// </remarks>
    /// <param name="key">
    /// Key that identifies a specific entity.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// The entity, or <see langword="null"/> if no entity with the given
    /// <paramref name="key"/> was found.
    /// </returns>
    Task<TEntity?> GetByIdAsync(
        int key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds the first entity of type <typeparamref name="TEntity"/>, sorted by
    /// <see cref="EntityBase.Id"/> in ascending order, that meets the search
    /// criteria defined in <paramref name="predicate"/>.
    /// </summary>
    /// <remarks>
    /// Note: Entities marked as hidden (<see cref="EntityBase.Hidden"/> is set
    /// to <see langword="true"/>) will <b>not</b> be fetched from the data
    /// source when using this method.
    /// </remarks>
    /// <param name="predicate">
    /// Repesents the search criteria as a predicate expression.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> for cancelling the operation.
    /// </param>
    /// <returns>
    /// The first entity that meets the search criteria, or
    /// <see langword="null"/> if no matches were found.
    /// </returns>
    Task<TEntity?> GetByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}
