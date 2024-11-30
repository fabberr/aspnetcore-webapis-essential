using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Filters;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController(CatalogDbContext dbContext) : CatalogApiController
{
    #region Constants
    private const string GetProductsActionName      = "GetProducts";

    private const string GetProductByIdActionName   = "GetProductById";

    private const string CreateProductActionName    = "CreateProduct";

    private const string UpdateProductActionName    = "UpdateProduct";

    private const string DeleteProductActionName    = "DeleteProduct";
    #endregion

    #region Fields
    private readonly CatalogDbContext _dbContext = dbContext;
    #endregion

    #region GET
    [HttpGet(Name = GetProductsActionName)]
    [ServiceFilter<ApiActionLoggingFilter>()]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(IOptionsSnapshot<ApiBehaviorSettings> options, uint? limit = null, uint offset = 0u)
    {
        var products = await _dbContext.Products.AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((int)offset)
            .Take((int)(limit ?? options.Value.DefaultItemsPerPage))
            .ToArrayAsync();

        if (products is null or { Length: 0 })
        {
            return NotFound();
        }

        return products;
    }

    [HttpGet(template: "{id:int}", Name = GetProductByIdActionName)]
    [ServiceFilter<ApiActionLoggingFilter>()]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = await _dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }
    #endregion

    #region POST
    [HttpPost(Name = CreateProductActionName)]
    [ServiceFilter<ApiActionLoggingFilter>()]
    public async Task<ActionResult<Product>> CreateProductAsync(Product product)
    {
        await _dbContext.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return CreatedAtRoute(
            routeName: GetProductByIdActionName,
            routeValues: new { id = product.Id },
            value: product
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = UpdateProductActionName)]
    [ServiceFilter<ApiActionLoggingFilter>()]
    public async Task<ActionResult<Product>> UpdateProductAsync(int id, Product product)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != product.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.ResourceIdMismatch, id, product.Id),
                modelStateDictionary: ModelState
            );
        }

        _dbContext.Entry(product).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return product;
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProductAsync))]
    [ServiceFilter<ApiActionLoggingFilter>()]
    public async Task<ActionResult<Product>> DeleteProductAsync(int id)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = await _dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        _dbContext.Remove(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }
    #endregion
}
