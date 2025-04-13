using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.Models.DTOs.Products;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Options;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class ProductsController(
    IUnitOfWork unitOfWork
)
    : CatalogApiController
{
    #region Dependencies
    private readonly IUnitOfWork _unit = unitOfWork;
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetProducts))]
    public async Task<ActionResult<IEnumerable<GetProductResponse>>> GetProducts(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        var products = await _unit.ProductRepository.QueryMultipleAsync(
            configureOptions: () => new PaginatedQueryOptions(
                Limit: (int)(limit ?? options.Value.DefaultItemsPerPage),
                Offset: (int)offset
            ),
            cancellationToken: cancellationToken
        );
        
        var response = products.ToGetResponse();

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "category/{categoryId:int}", Name = nameof(GetProductsByCategoryId))]
    public async Task<ActionResult<IEnumerable<GetProductResponse>>> GetProductsByCategoryId(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromRoute] int categoryId,
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        if (categoryId <= 0)
        {
            ModelState.TryAddModelError(nameof(categoryId), string.Format(Messages.Validation.InvalidValue, categoryId));
            return ValidationProblem(ModelState);
        }

        var products = await _unit.ProductRepository.QueryMultipleByCategoryIdAsync(
            categoryKey: categoryId,
            configureOptions: () => new PaginatedQueryOptions(
                Limit: (int)(limit ?? options.Value.DefaultItemsPerPage),
                Offset: (int)offset
            ),
            cancellationToken: cancellationToken
        );

        var response = products.ToGetResponse();

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public async Task<ActionResult<GetProductResponse>> GetProductById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var product = await _unit.ProductRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (product is null)
        {
            return NotFound();
        }

        return product.ToGetResponse();
    }
    #endregion

    #region POST
    [HttpPost(Name = nameof(CreateProduct))]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct(
        [FromBody] CreateProductRequest createProductRequest,
        CancellationToken cancellationToken = default
    )
    {
        var createdProduct = await _unit.ProductRepository.CreateAsync(
            entity: createProductRequest.ToEntity(),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        var response = createdProduct.ToCreateResponse();

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateProductById))]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProductById(
        [FromRoute] int id,
        [FromBody] UpdateProductRequest updateProductRequest,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != updateProductRequest.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, updateProductRequest.Id),
                modelStateDictionary: ModelState
            );
        }

        var product = updateProductRequest.ToEntity();

        var currentProduct = await _unit.ProductRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentProduct is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(product.Name) is not true && product.Name != currentProduct.Name)
        {
            currentProduct.Name = product.Name;
        }
        if (string.IsNullOrWhiteSpace(product.Description) is not true && product.Description != currentProduct.Description)
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
        if (string.IsNullOrWhiteSpace(product.ImageUri) is not true && product.ImageUri != currentProduct.ImageUri)
        {
            currentProduct.ImageUri = product.ImageUri;
        }
        if (product.CategoryId != default && product.CategoryId != currentProduct.CategoryId)
        {
            currentProduct.CategoryId = product.CategoryId;
        }

        var updatedProduct = await _unit.ProductRepository.UpdateAsync(
            entity: currentProduct,
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return updatedProduct.ToUpdateResponse();
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProductById))]
    public async Task<ActionResult<DeleteProductResponse>> DeleteProductById(
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

        var removedProduct = await _unit.ProductRepository.RemoveByIdAsync(
            key: id,
            strategy: options.Value.RemoveStrategy,
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        if (removedProduct is null)
        {
            return NotFound();
        }

        return removedProduct.ToDeleteResponse();
    }
    #endregion
}
