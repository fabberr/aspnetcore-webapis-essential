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
public sealed class CategoriesController(
    ICategoryRepository categoryRepository
)
    : CatalogApiController
{
    #region Dependencies
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    #endregion

    #region GET
    [HttpGet(Name = nameof(GetCategories))]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        var categories = await _categoryRepository.QueryMultipleAsync(
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset,
            cancellationToken: cancellationToken
        );

        if (categories.Any() is false)
        {
            return NoContent();
        }

        return categories.ToArray();
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryById))]
    public async Task<ActionResult<Category>> GetCategoryById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = await _categoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet(template: "{id:int}/products", Name = nameof(GetProductsByCategory))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(
        IOptionsSnapshot<ApiBehaviorSettings> options,
        [FromRoute] int id,
        [FromQuery] uint? limit = null,
        [FromQuery] uint offset = 0u,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var products = await _categoryRepository.GetProductsByCategoryIdAsync(
            categoryKey: id,
            limit: limit ?? options.Value.DefaultItemsPerPage,
            offset: offset,
            cancellationToken: cancellationToken
        );

        return products.ToArray();
    }
    #endregion

    #region POST
    [HttpPost(Name = nameof(CreateCategory))]
    public async Task<ActionResult<Category>> CreateCategory(
        [FromBody] Category category,
        CancellationToken cancellationToken = default
    )
    {
        var createdCategory = await _categoryRepository.CreateAsync(
            entity: category,
            cancellationToken: cancellationToken
        );

        return CreatedAtRoute(
            routeName: nameof(GetCategoryById),
            routeValues: new { id = category.Id },
            value: createdCategory
        );
    }
    #endregion

    #region PUT
    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategoryAsync))]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(
        [FromRoute] int id,
        [FromBody] Category category,
        CancellationToken cancellationToken = default
    )
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        if (id != category.Id)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(
                detail: string.Format(Messages.Validation.SpecifiedKeyDoesNotMatchEntityKey, id, category.Id),
                modelStateDictionary: ModelState
            );
        }

        var currentCategory = await _categoryRepository.FindByIdAsync(
            key: id,
            cancellationToken: cancellationToken
        );

        if (currentCategory is null)
        {
            return NotFound();
        }

        if (category.Name is not "" && category.Name != currentCategory.Name)
        {
            currentCategory.Name = category.Name;
        }
        if (category.ImageUri is not "" && category.ImageUri != currentCategory.ImageUri)
        {
            currentCategory.ImageUri = category.ImageUri;
        }

        return await _categoryRepository.UpdateAsync(
            entity: currentCategory,
            cancellationToken: cancellationToken
        );
    }
    #endregion

    #region DELETE
    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategory))]
    public async Task<ActionResult<Category>> DeleteCategory(
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

        var removedCategory = await _categoryRepository.RemoveByIdAsync(
            key: id,
            strategy: options.Value.RemoveStrategy,
            cancellationToken: cancellationToken
        );

        if (removedCategory is null)
        {
            return NotFound();
        }

        return removedCategory;
    }
    #endregion
}

// @todo: (when DTOs are implemented) make all properties except `id` optional for PUT routes
// @todo: Paginated response DTOs for Get actions
