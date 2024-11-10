using System.Collections.Generic;
using System.Linq;
using Catalog.Api.Constants;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController(CatalogDbContext dbContext) : CatalogApiController
{
    private readonly CatalogDbContext _dbContext = dbContext;

    [HttpGet(Name = nameof(ListProducts))]
    public ActionResult<IEnumerable<Product>> ListProducts(uint limit = 10u, uint offset = 0u)
    {
        var products = _dbContext.Products.AsNoTracking()
            .Skip((int)offset)
            .Take((int)limit)
            .OrderBy(p => p.Id)
            .ToArray();

        if (products is null or { Length: 0 })
        {
            return NotFound();
        }

        return products;
    }

    [HttpPost(Name = nameof(CreateProduct))]
    public ActionResult<Product> CreateProduct(Product product)
    {
        _dbContext.Add(product);
        _dbContext.SaveChanges();

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { id = product.Id },
            value: product
        );
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public ActionResult<Product> GetProductById(int id)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = _dbContext.Products.Find(id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPut(template: "{id:int}", Name = nameof(UpdateProduct))]
    public ActionResult<Product> UpdateProduct(int id, Product product)
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
        _dbContext.SaveChanges();

        return product;
    }

    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProduct))]
    public ActionResult<Product> DeleteProduct(int id)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = _dbContext.Products.Find(id);

        if (product is null)
        {
            return NotFound();
        }

        _dbContext.Remove(product);
        _dbContext.SaveChanges();

        return product;
    }
}
