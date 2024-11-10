using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Constants;
using Catalog.Core.Context;
using Catalog.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[Route("api/[controller]")]
public sealed class CategoriesController(CatalogDbContext dbContext) : CatalogApiController
{
    private readonly CatalogDbContext _dbContext = dbContext;

    [HttpGet(Name = nameof(ListCategoriesAsync))]
    public async Task<ActionResult<IEnumerable<Category>>> ListCategoriesAsync(bool includeProducts, uint limit = 10u, uint offset = 0u)
    {
        var categoriesQuery = _dbContext.Categories.AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((int)offset)
            .Take((int)limit);

        var categories = includeProducts
            ? await categoriesQuery.Include(c => c.Products).ToArrayAsync()
            : await categoriesQuery.ToArrayAsync();

        if (categories is null or { Length: 0 })
        {
            return NotFound();
        }

        return categories;
    }

    [HttpPost(Name = nameof(CreateCategoryAsync))]
    public async Task<ActionResult<Category>> CreateCategoryAsync(Category category)
    {
        await _dbContext.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return CreatedAtRoute(
            routeName: nameof(GetCategoryByIdAsync),
            routeValues: new { id = category.Id },
            value: category
        );
    }

    [HttpGet(template: "{id:int}", Name = nameof(GetCategoryByIdAsync))]
    public async Task<ActionResult<Category>> GetCategoryByIdAsync(int id, bool includeProducts)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = includeProducts
            ? await _dbContext.Categories.AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id)
            : await _dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return category;
    }

    [HttpGet(template: "{id:int}/products", Name = nameof(ListCategoryProductsAsync))]
    public async Task<ActionResult<IEnumerable<Product>>> ListCategoryProductsAsync(int id, uint limit = 10u, uint offset = 0u)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var products = await _dbContext.Categories.AsNoTracking()
            .Where(c => c.Id == id)
            .Join(
                _dbContext.Products.AsNoTracking(),
                category => category.Id, product => product.CategoryId,
                (_, product) => product
            )
            .OrderBy(p => p.Id)
            .Skip((int)offset)
            .Take((int)limit)
            .ToArrayAsync();

        if (products is null or { Length: 0 })
        {
            return NotFound();
        }

        return products;
    }

    [HttpPut(template: "{id:int}", Name = nameof(UpdateCategoryAsync))]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(int id, Category category)
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
                detail: string.Format(Messages.Validation.ResourceIdMismatch, id, category.Id),
                modelStateDictionary: ModelState
            );
        }

        _dbContext.Entry(category).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return category;
    }

    [HttpDelete(template: "{id:int}", Name = nameof(DeleteCategoryAsync))]
    public async Task<ActionResult<Category>> DeleteCategoryAsync(int id)
    {
        if (id <= 0)
        {
            ModelState.TryAddModelError(nameof(id), string.Format(Messages.Validation.InvalidValue, id));
            return ValidationProblem(ModelState);
        }

        var category = await _dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        _dbContext.Remove(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }
}
