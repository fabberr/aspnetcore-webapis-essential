using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Models.Abstractions;

/// <summary>
/// Defines a static method for creating <typeparamref name="TSelf"/> instances
/// from <typeparamref name="TEntity"/> entities.
/// </summary>
/// <typeparam name="TSelf">
/// Type to perform the the conversion to.
/// </typeparam>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IMappableFromEntity<TSelf, TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Creates an instance of <typeparamref name="TSelf"/> from a
    /// <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <param name="entity">
    /// Entity to create the instance of <typeparamref name="TSelf"/> from.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="TSelf"/> created from the
    /// <paramref name="entity"/> instance.
    /// </returns>
    abstract static TSelf FromEntity(TEntity entity);
}

/// <summary>
/// Defines a static method for creating arrays of <typeparamref name="TSelf"/>
/// instances from a collection of <typeparamref name="TEntity"/> entities.
/// </summary>
/// <remarks>
/// This interface is typically implemented as an extension of the
/// <see cref="IMappableFromEntity{TSelf, TEntity}"/> interface.
/// </remarks>
/// <typeparam name="TSelf">
/// Type to perform the the conversion to.
/// </typeparam>
/// <typeparam name="TEntity">
/// Type of the entity.
/// </typeparam>
public interface IMappableFromEntityCollection<TSelf, TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Creates an array of <typeparamref name="TSelf"/> instances from a
    /// collection of <typeparamref name="TEntity"/> entities.
    /// </summary>
    /// <remarks>
    /// Elements that point to <see langword="null"/> in the source collection
    /// will be skipped.
    /// </remarks>
    /// <param name="entities">
    /// Collection of entities to create an array of <typeparamref name="TSelf"/>
    /// instances from.
    /// </param>
    /// <returns>
    /// An array of <typeparamref name="TSelf"/> instances created from the
    /// <paramref name="entities"/> collection, or <see langword="null"/> if
    /// every element in the collection pointed to <see langword="null"/>.
    /// </returns>
    abstract static TSelf[]? FromEntities(IEnumerable<TEntity> entities);
}

/// <summary>
/// Static helper class providing a default implementation for
/// <see cref="IMappableFromEntityCollection{TSelf, TEntity}.FromEntities(IEnumerable{TEntity})"/>.
/// </summary>
public static class Mapper
{
    public static TSelf[]? FromEntities<TSelf, TEntity>(IEnumerable<TEntity> entities)
        where TSelf : IMappableFromEntity<TSelf, TEntity>
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        var convertibleEntities = entities.Where(static (entity) => entity is not null);

        if (convertibleEntities.Any() is false)
        {
            return null;
        }
        
        return [.. convertibleEntities.Select(static (entity) => TSelf.FromEntity(entity))];
    }
}
