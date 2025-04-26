using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.DTOs.Products;
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

    #region POST
    [HttpPost(Name = nameof(CreateProduct))]
    public async Task<ActionResult<CreateResponse>> CreateProduct(
        [FromBody] CreateRequest createProductRequest,
        CancellationToken cancellationToken = default
    )
    {
        var createdProduct = await _unit.ProductRepository.CreateAsync(
            entity: createProductRequest.ToEntity(),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        var response = CreateResponse.FromEntity(createdProduct);

        return CreatedAtRoute(
            routeName: nameof(GetProductById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetProducts))]
    public async Task<ActionResult<IEnumerable<ReadResponse>>> GetProducts(
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
        
        var response = ReadResponse.FromEntities(products);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "category/{categoryId:int}", Name = nameof(GetProductsByCategoryId))]
    public async Task<ActionResult<IEnumerable<ReadResponse>>> GetProductsByCategoryId(
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

        var response = ReadResponse.FromEntities(products);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetProductById))]
    public async Task<ActionResult<ReadResponse>> GetProductById(
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

        return ReadResponse.FromEntity(product);
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateProductById))]
    public async Task<ActionResult<UpdateResponse>> UpdateProductById(
        [FromRoute] int id,
        [FromBody] UpdateRequest updateProductRequest,
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

        var updatedProduct = await _unit.ProductRepository.UpdateAsync(
            entity: currentProduct.OverwriteWith(product),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return UpdateResponse.FromEntity(updatedProduct);
    }
    #endregion

    #region PATCH
    [HttpPatch(template: "{id:int}", Name = nameof(PatchProductById))]
    public async Task<ActionResult<PatchResponse>> PatchProductById(
        [FromRoute] int id,
        [FromBody] PatchRequest patchRequest,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != patchRequest.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, patchRequest.Id),
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

        return PatchResponse.FromEntity(updatedProduct);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteProductById))]
    public async Task<ActionResult<DeleteResponse>> DeleteProductById(
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

        return DeleteResponse.FromEntity(removedProduct);
    }
    #endregion
}
