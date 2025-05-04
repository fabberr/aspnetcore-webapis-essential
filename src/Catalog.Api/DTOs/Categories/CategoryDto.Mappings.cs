using System.Collections.Generic;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Entities.Abstractions;

namespace Catalog.Api.DTOs.Categories;

#region POST
public sealed partial record CreateCategoryRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record CreateCategoryResponse : IMappableFromEntity<CreateCategoryResponse, Category>
{
    private CreateCategoryResponse(Category category) : base(category) {}

    public static CreateCategoryResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region GET
public sealed partial record ReadCategoryResponse
    : IMappableFromEntity<ReadCategoryResponse, Category>
    , IMappableFromEntityCollection<ReadCategoryResponse, Category>
{
    private ReadCategoryResponse(Category category) : base(category) {}

    public static ReadCategoryResponse FromEntity(Category entity) => new(entity);

    public static ReadCategoryResponse[]? FromEntities(IEnumerable<Category> sources) => Mapper.FromEntities<ReadCategoryResponse, Category>(sources);
}
#endregion

#region PUT
public sealed partial record UpdateCategoryRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Id = Id,
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record UpdateCategoryResponse : IMappableFromEntity<UpdateCategoryResponse, Category>
{
    private UpdateCategoryResponse(Category category) : base(category) {}

    public static UpdateCategoryResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region PATCH
public sealed partial record PatchCategoryRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Id = Id,
        Name = Name!,
        ImageUri = ImageUri!,
    };
}

public sealed partial record PatchCategoryResponse : IMappableFromEntity<PatchCategoryResponse, Category>
{
    private PatchCategoryResponse(Category category) : base(category) {}

    public static PatchCategoryResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region DELETE
public sealed partial record DeleteCategoryResponse : IMappableFromEntity<DeleteCategoryResponse, Category>
{
    private DeleteCategoryResponse(Category category) : base(category) {}

    public static DeleteCategoryResponse FromEntity(Category entity) => new(entity);
}
#endregion
