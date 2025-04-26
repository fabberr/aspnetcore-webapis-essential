using System.Collections.Generic;

namespace Catalog.Core.Models.Entities;

public sealed partial class Category
{
    #region Category 1..* Product
    public ICollection<Product>? Products { get; set; } = null;
    #endregion
}