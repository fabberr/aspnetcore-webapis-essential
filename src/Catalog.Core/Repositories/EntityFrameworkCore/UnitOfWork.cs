using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Core.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Core.Repositories.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="IUnitOfWork"/> for Entity Framework Core.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
/// </remarks>
/// <param name="serviceProvider">
/// A <see cref="IServiceProvider"/> instance for resolving instance.
/// </param>
/// <param name="dbContext">
/// An Entity Framework Core <see cref="DbContext"/>.
/// </param>
public sealed class UnitOfWork(
    IServiceProvider serviceProvider,
    DbContext dbContext
)
    : IUnitOfWork
{
    #region Fields
    private ICategoryRepository? _categoryRepository = null;
    private IProductRepository? _productRepository = null;
    #endregion

    #region Dependencies
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly DbContext _dbContext = dbContext;
    #endregion

    #region IUnitOfWork
    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public ICategoryRepository CategoryRepository => _categoryRepository
        ??= _serviceProvider.GetRequiredService<ICategoryRepository>();

    public IProductRepository ProductRepository => _productRepository
        ??= _serviceProvider.GetRequiredService<IProductRepository>();
    #endregion
}
