using Catalog.Core.Abstractions.Repositories.Generic.Interfaces;
using Catalog.Core.Models.Entities;

namespace Catalog.Core.Repositories.Interfaces;

/// <inheritdoc/>
public interface IProductRepository : IRepository<Product, int>;
