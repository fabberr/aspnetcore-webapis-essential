using Catalog.Core.Models.Entities;

namespace Catalog.Core.Enums;

/// <summary>
/// The supported strategies for deleting entities from the data source.
/// </summary>
public enum DeleteStrategy
{
    /// <summary>
    /// Physically delete entities from the data source when executing a delete
    /// operation.
    /// </summary>
    Delete = default,

    /// <summary>
    /// Hide entities instead of deleting them when executing a delete operation
    /// by setting the <see cref="EntityBase.Hidden"/> property to
    /// <see langword="true"/> and updating their state on the data source.
    /// </summary>
    Hide,
}
