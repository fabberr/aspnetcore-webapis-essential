using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
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
    private readonly CatalogDbContext _dbContext = dbContext;

    [HttpGet(Name = nameof(ListProductsAsync))]
    public async Task<ActionResult<IEnumerable<Product>>> ListProductsAsync(IOptionsSnapshot<ApiBehaviorSettings> options, uint? limit = null, uint offset = 0u)
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

    [HttpPost(Name = nameof(CreateProductAsync))]
    public async Task<ActionResult<Product>> CreateProductAsync(Product product)
    {
        await _dbContext.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return CreatedAtRoute(
            routeName: nameof(GetProductByIdAsync),
            routeValues: new { id = product.Id },
            value: product
        );
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductByIdAsync))]
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

    [HttpPut(template: "{id:int}", Name = nameof(UpdateProductAsync))]
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

    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProductAsync))]
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
}
