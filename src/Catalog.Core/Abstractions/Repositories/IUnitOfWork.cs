using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Core.Abstractions.Repositories;

/// <summary>
/// Represents a Unit of Work responsible for managing changes across multiple
/// Repositories instances
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persist the changes to the data source.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the
    /// operation to complete.
    /// </param>
    Task CommitChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an instance of the Category Repository.
    /// </summary>
    /// <remarks>
    /// Instances of this Category Repository will be shared across the lifetime
    /// of this Unit of Work instance.
    /// </remarks>
    public ICategoryRepository CategoryRepository { get; }

    /// <summary>
    /// Gets an instance of the Product Repository.
    /// </summary>
    /// <remarks>
    /// Instances of this Product Repository will be shared across the lifetime
    /// of this Unit of Work instance.
    /// </remarks>
    public IProductRepository ProductRepository { get; }
}
