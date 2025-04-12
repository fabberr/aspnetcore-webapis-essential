using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Abstractions;
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
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        var products = await _productRepository.QueryMultipleAsync(
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset,
            cancellationToken: cancellationToken
        );

        return products.ToArray();
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public async Task<ActionResult<Product>> GetProductById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = await _productRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

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
        [FromBody] Product product,
        CancellationToken cancellationToken = default
    )
    {
        var createdProduct = await _productRepository.CreateAsync(
            entity: product,
            cancellationToken: cancellationToken
        );

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
        [FromRoute] int id,
        [FromBody] Product product,
        CancellationToken cancellationToken = default
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

        var currentProduct = await _productRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentProduct is null)
        {
            return NotFound();
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

        return await _productRepository.UpdateAsync(
            entity: currentProduct,
            cancellationToken: cancellationToken
        );
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProduct))]
    public async Task<ActionResult<Product>> DeleteProduct(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var deletedProduct = await _productRepository.DeleteByIdAsync(
            key: id,
            strategy: options.Value.DeleteStrategy,
            cancellationToken: cancellationToken
        );

        if (deletedProduct is null)
        {
            return NotFound();
        }

        return deletedProduct;
    }
    #endregion
}
