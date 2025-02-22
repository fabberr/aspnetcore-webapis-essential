using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Models.Entities;

/// <summary>
/// Represents a product for sale.<br/>
/// This type cannot be inherited.
/// </summary>
[Table("Products")]
public sealed class Product : EntityBase
{
    /// <summary>
    /// The name of this product.
    /// </summary>
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of this product.
    /// </summary>
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The price of this product.
    /// </summary>
    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; } = default;

    /// <summary>
    /// The current quantity in stock of this product.
    /// </summary>
    [Required]
    public float Stock { get; set; } = default;

    /// <summary>
    /// The URI of this product's thumbnail image.
    /// </summary>
    [Required]
    [StringLength(300)]
    public string ImageUri { get; set; } = string.Empty;

    #region Category 1..* Product
    /// <summary>
    /// The Id of the category this product belongs to.
    /// </summary>
    public int CategoryId { get; set; } = default;

    /// <summary>
    /// The category this product belongs to.
    /// </summary>
    [JsonIgnore]
    public Category? Category { get; set; } = null;
    #endregion
}
