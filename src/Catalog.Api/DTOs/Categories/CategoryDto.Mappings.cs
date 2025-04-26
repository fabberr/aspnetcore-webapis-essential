using System.Collections.Generic;
using Catalog.Core.Models.Entities;
using Catalog.Core.Models.Entities.Abstractions;

namespace Catalog.Api.DTOs.Categories;

#region POST
public sealed partial record CreateRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record CreateResponse : IMappableFromEntity<CreateResponse, Category>
{
    private CreateResponse(Category category) : base(category) {}

    public static CreateResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region GET
public sealed partial record ReadResponse
    : IMappableFromEntity<ReadResponse, Category>
    , IMappableFromEntityCollection<ReadResponse, Category>
{
    private ReadResponse(Category category) : base(category) {}

    public static ReadResponse FromEntity(Category entity) => new(entity);

    public static ReadResponse[]? FromEntities(IEnumerable<Category> sources) => Mapper.FromEntities<ReadResponse, Category>(sources);
}
#endregion

#region PUT
public sealed partial record UpdateRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Id = Id,
        Name = Name,
        ImageUri = ImageUri,
    };
}

public sealed partial record UpdateResponse : IMappableFromEntity<UpdateResponse, Category>
{
    private UpdateResponse(Category category) : base(category) {}

    public static UpdateResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region PATCH
public sealed partial record PatchRequest : IMappableToEntity<Category>
{
    public Category ToEntity() => new() {
        Id = Id,
        Name = Name!,
        ImageUri = ImageUri!,
    };
}

public sealed partial record PatchResponse : IMappableFromEntity<PatchResponse, Category>
{
    private PatchResponse(Category category) : base(category) {}

    public static PatchResponse FromEntity(Category entity) => new(entity);
}
#endregion

#region DELETE
public sealed partial record DeleteResponse : IMappableFromEntity<DeleteResponse, Category>
{
    private DeleteResponse(Category category) : base(category) {}

    public static DeleteResponse FromEntity(Category entity) => new(entity);
}
#endregion
