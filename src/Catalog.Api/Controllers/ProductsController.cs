using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController(
    IProductRepository productRepository
)
    : CatalogApiController
{
    #region Dependencies
    private readonly IProductRepository _productRepository = productRepository;
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetProducts))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        uint? limit = null,
        uint offset = 0u
    )
    {
        var products = await _productRepository.GetAsync(
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset
        );

        if (products.Any() is false)
        {
            return NotFound();
        }

        return products.ToArray();
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public async Task<ActionResult<Product>> GetProductById(
        int id
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = await _productRepository.GetAsync(key: id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }
    #endregion

    #region POST
    [HttpPost(Name = nameof(CreateProduct))]
    public async Task<ActionResult<Product>> CreateProduct(
        Product product
    )
    {
        var createdProduct = await _productRepository.CreateAsync(entity: product);

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { id = product.Id },
            value: createdProduct
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateProduct))]
    public async Task<ActionResult<Product>> UpdateProduct(
        int id,
        Product product
    )
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
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, product.Id),
                modelStateDictionary: ModelState
            );
        }

        var currentProduct = await _productRepository.GetAsync(key: id);
        if (currentProduct is null)
        {
            return NotFound();
        }

        {
            currentProduct.Hidden = product.Hidden;
        }
        if (product.Name is not "" && product.Name != currentProduct.Name)
        {
            currentProduct.Name = product.Name;
        }
        if (product.Description is not "" && product.Description != currentProduct.Description)
        {
            currentProduct.Description = product.Description;
        }
        if (product.Price != default && product.Price != currentProduct.Price)
        {
            currentProduct.Price = product.Price;
        }
        if (product.Stock != default && product.Stock != currentProduct.Stock)
        {
            currentProduct.Stock = product.Stock;
        }
        if (product.ImageUri is not "" && product.ImageUri != currentProduct.ImageUri)
        {
            currentProduct.ImageUri = product.ImageUri;
        }

        return await _productRepository.UpdateAsync(entity: currentProduct);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProduct))]
    public async Task<ActionResult<Product>> DeleteProduct(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        int id
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var currentProduct = await _productRepository.GetAsync(key: id);
        if (currentProduct is null)
        {
            return NotFound();
        }

        if (options.Value.DeleteBehavior is ApiDeleteBehavior.Logical)
        {
            currentProduct.Hidden = true;
        }

        var deletedCategory = await (options.Value.DeleteBehavior switch {
            ApiDeleteBehavior.Physical
                => _productRepository.DeleteAsync(entity: currentProduct),
            ApiDeleteBehavior.Logical
                => _productRepository.UpdateAsync(entity: currentProduct),
            _ => throw new System.InvalidOperationException(),
        });

        return deletedCategory;
    }
    #endregion
}
