using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.DTOs;
using Catalog.Api.DTOs.Pagination;
using Catalog.Core.Abstractions.Repositories;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class CategoriesController(
    IUnitOfWork unitOfWork
)
    : CatalogApiController
{
    #region Dependencies
    private readonly IUnitOfWork _unit = unitOfWork;
    #endregion

    #region POST
    [HttpPost(Name = nameof(CreateCategory))]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(
        [FromBody]
        CreateCategoryRequest createRequest,

        CancellationToken cancellationToken = default
    )
    {
        var createdCategory = await _unit.CategoryRepository.CreateAsync(
            entity: createRequest.ToEntity(),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        var response = CreateCategoryResponse.FromEntity(createdCategory);

        return CreatedAtRoute(
            routeName: nameof(GetCategoryById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<ActionResult<IEnumerable<ReadCategoryResponse>>> GetCategories(
        [FromQuery]
        PaginationQueryParameters parameters,

        CancellationToken cancellationToken = default
    )
    {
        var categories = await _unit.CategoryRepository.QueryMultipleAsync(
            configureOptions: () => new QueryOptions(parameters),
            cancellationToken: cancellationToken
        );

        if (categories.Any() is false)
        {
            return NoContent();
        }

        var response = ReadCategoryResponse.FromEntities(categories);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<ReadCategoryResponse>> GetCategoryById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        CancellationToken cancellationToken = default
    )
    {
        var category = await _unit.CategoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (category is null)
        {
            return NotFound();
        }

        return ReadCategoryResponse.FromEntity(category);
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategoryById))]
    public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategoryById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        [FromBody] UpdateCategoryRequest updateRequest,
        CancellationToken cancellationToken = default
    )
    {
        var category = updateRequest.ToEntity();

        var currentCategory = await _unit.CategoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentCategory is null)
        {
            return NotFound();
        }

        var updatedCategory = await _unit.CategoryRepository.UpdateAsync(
            entity: currentCategory.OverwriteWith(category),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return UpdateCategoryResponse.FromEntity(updatedCategory);
    }
    #endregion

    #region PATCH
    [HttpPatch(template: "{id:int}", Name = nameof(PatchCategoryById))]
    public async Task<ActionResult<PatchCategoryResponse>> PatchCategoryById(
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        [FromBody] PatchCategoryRequest patchRequest,
        CancellationToken cancellationToken = default
    )
    {
        var category = patchRequest.ToEntity();

        var currentCategory = await _unit.CategoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentCategory is null)
        {
            return NotFound();
        }

        var updatedCategory = await _unit.CategoryRepository.UpdateAsync(
            entity: currentCategory.MergeWith(category),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        return PatchCategoryResponse.FromEntity(updatedCategory);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategoryById))]
    public async Task<ActionResult<DeleteCategoryResponse>> DeleteCategoryById(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromRoute]
        [Range(minimum: 1, maximum: int.MaxValue)]
        int id,

        CancellationToken cancellationToken = default
    )
    {
        var removedCategory = await _unit.CategoryRepository.RemoveByIdAsync(
            key: id,
            strategy: options.Value.RemoveStrategy,
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        if (removedCategory is null)
        {
            return NotFound();
        }

        return DeleteCategoryResponse.FromEntity(removedCategory);
    }
    #endregion
}
