using Catalog.Core.Models.Entities;

namespace Catalog.Core.Abstractions.Mapping;

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
