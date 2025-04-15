using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.DTOs.Categories;

public abstract record CategoryDto(
    int Id = default,
    string Name = "",
    string ImageUri = "",
    DateTime CreatedAt = default,
    DateTime? UpdatedAt = default
);

#region GET
public sealed partial record ReadResponse : CategoryDto;
#endregion

#region POST
public sealed partial record CreateRequest(
    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(300)]
    string ImageUri
);

public sealed partial record CreateResponse : CategoryDto;
#endregion

#region PUT
public sealed partial record UpdateRequest(
    [Required]
    int Id,

    [Required]
    [StringLength(80)]
    string Name,

    [Required]
    [StringLength(300)]
    string ImageUri
);

public sealed partial record UpdateResponse : CategoryDto;
#endregion

#region DELETE
public sealed partial record DeleteResponse() : CategoryDto;
#endregion
