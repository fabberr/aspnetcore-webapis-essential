using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.DTOs;
using Catalog.Api.DTOs.Pagination;
using Catalog.Core.Abstractions.Repositories;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories;
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

    #region POST
    [HttpPost(Name = nameof(CreateProduct))]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct(
        [FromBody]
        CreateProductRequest createProductRequest,

        CancellationToken cancellationToken = default
    )
    {
        var createdProduct = await _unit.ProductRepository.CreateAsync(
            entity: createProductRequest.ToEntity(),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        var response = CreateProductResponse.FromEntity(createdProduct);

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetProducts))]
    public async Task<ActionResult<IEnumerable<ReadProductResponse>>> GetProducts(
        [FromQuery]
        PaginationQueryParameters parameters,

        CancellationToken cancellationToken = default
    )
    {
        var products = await _unit.ProductRepository.QueryMultipleAsync(
            configureOptions: () => new PaginatedQueryOptions(parameters),
            cancellationToken: cancellationToken
        );
        
        var response = ReadProductResponse.FromEntities(products);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "category/{categoryId:int}", Name = nameof(GetProductsByCategoryId))]
    public async Task<ActionResult<IEnumerable<ReadProductResponse>>> GetProductsByCategoryId(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int categoryId,

        [FromQuery]
        PaginationQueryParameters parameters,

        CancellationToken cancellationToken = default
    )
    {
        var products = await _unit.ProductRepository.QueryMultipleByCategoryIdAsync(
            categoryKey: categoryId,
            configureOptions: () => new PaginatedQueryOptions(parameters),
            cancellationToken: cancellationToken
        );

        var response = ReadProductResponse.FromEntities(products);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public async Task<ActionResult<ReadProductResponse>> GetProductById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        CancellationToken cancellationToken = default
    )
    {
        var product = await _unit.ProductRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (product is null)
        {
            return NotFound();
        }

        return ReadProductResponse.FromEntity(product);
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateProductById))]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProductById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        [FromBody]
        UpdateProductRequest updateProductRequest,

        CancellationToken cancellationToken = default
    )
    {
        if (id != updateProductRequest.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.KeyDoesNotMatchEntityKey, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.KeyDoesNotMatchEntityKey, id, updateProductRequest.Id),
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

        var updatedProduct = await _unit.ProductRepository.UpdateAsync(
            entity: currentProduct.OverwriteWith(product),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return UpdateProductResponse.FromEntity(updatedProduct);
    }
    #endregion

    #region PATCH
    [HttpPatch(template: "{id:int}", Name = nameof(PatchProductById))]
    public async Task<ActionResult<PatchProductResponse>> PatchProductById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        [FromBody]
        PatchProductRequest patchRequest,

        CancellationToken cancellationToken = default
    )
    {
        if (id != patchRequest.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.KeyDoesNotMatchEntityKey, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.KeyDoesNotMatchEntityKey, id, patchRequest.Id),
                modelStateDictionary: ModelState
            );
        }

        var product = patchRequest.ToEntity();

        var currentProduct = await _unit.ProductRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentProduct is null)
        {
            return NotFound();
        }

        var updatedProduct = await _unit.ProductRepository.UpdateAsync(
            entity: currentProduct.MergeWith(product),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return PatchProductResponse.FromEntity(updatedProduct);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProductById))]
    public async Task<ActionResult<DeleteProductResponse>> DeleteProductById(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        CancellationToken cancellationToken = default
    )
    {
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

        return DeleteProductResponse.FromEntity(removedProduct);
    }
    #endregion
}
