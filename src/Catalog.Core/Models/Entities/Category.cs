using System;

namespace Catalog.Core.Models.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public Uri? ImageUri { get; set; } = null;
}
