using System;

namespace Catalog.Core.Models.Entities;

public abstract class EntityBase
{
    public int Id { get; init; } = default;

    public DateTime CreatedAt { get; set; } = default;

    public DateTime? UpdatedAt { get; set; } = null;

    public bool Hidden { get; set; } = default;
}
