using System;

namespace Catalog.Core.Models.Entities;

public class Product : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; } = default;

    public float Stock { get; set; } = default;

    public Uri? ImageUri { get; set; } = null;
}
