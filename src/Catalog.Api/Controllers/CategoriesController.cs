using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Api.DTOs.Categories;
using Catalog.Core.Models.Options;
using Catalog.Core.Models.Settings;
using Catalog.Core.Repositories.Abstractions;
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
    public async Task<ActionResult<CreateResponse>> CreateCategory(
        [FromBody] CreateRequest createRequest,
        CancellationToken cancellationToken = default
    )
    {
        var createdCategory = await _unit.CategoryRepository.CreateAsync(
            entity: createRequest.ToEntity(),
            cancellationToken: cancellationToken
        );
        await _unit.CommitChangesAsync(cancellationToken: cancellationToken);

        var response = CreateResponse.FromEntity(createdCategory);

        return CreatedAtRoute(
            routeName: nameof(GetCategoryById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<ActionResult<IEnumerable<ReadResponse>>> GetCategories(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        var categories = await _unit.CategoryRepository.QueryMultipleAsync(
            configureOptions: () => new PaginatedQueryOptions(
                Limit: (int)(limit ?? options.Value.DefaultItemsPerPage),
                Offset: (int)offset
            ),
            cancellationToken: cancellationToken
        );

        if (categories.Any() is false)
        {
            return NoContent();
        }

        var response = ReadResponse.FromEntities(categories);

        if (response is null or { Length: 0 })
        {
            return NoContent();
        }

        return response;
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<ReadResponse>> GetCategoryById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = await _unit.CategoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (category is null)
        {
            return NotFound();
        }

        return ReadResponse.FromEntity(category);
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategoryById))]
    public async Task<ActionResult<UpdateResponse>> UpdateCategoryById(
        [FromRoute] int id,
        [FromBody] UpdateRequest updateRequest,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != updateRequest.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, updateRequest.Id),
                modelStateDictionary: ModelState
            );
        }

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

        return UpdateResponse.FromEntity(updatedCategory);
    }
    #endregion

    #region PATCH
    [HttpPatch(template: "{id:int}", Name = nameof(PatchCategoryById))]
    public async Task<ActionResult<PatchResponse>> PatchCategoryById(
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

        return PatchResponse.FromEntity(updatedCategory);
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategoryById))]
    public async Task<ActionResult<DeleteResponse>> DeleteCategoryById(
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

        return DeleteResponse.FromEntity(removedCategory);
    }
    #endregion
}
