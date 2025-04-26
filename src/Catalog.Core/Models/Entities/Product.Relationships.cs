namespace Catalog.Core.Models.Entities;

public sealed partial class Product
{
    #region Category 1..* Product
    public int CategoryId { get; set; } = default;

    public Category? Category { get; set; } = null;
    #endregion
}