using Catalog.Core.Models.Entities;

namespace Catalog.Core.Enums;

/// <summary>
/// The supported strategies for removing entities from the data source.
/// </summary>
public enum RemoveStrategy
{
    /// <summary>
    /// Delete entities from the data source when executing a remove operation.
    /// </summary>
    Delete = default,

    /// <summary>
    /// Hide entities instead of deleting them when executing a remove operation
    /// by setting the <see cref="EntityBase.Hidden"/> property to
    /// <see langword="true"/> and updating their state on the data source.
    /// </summary>
    Hide,
}
