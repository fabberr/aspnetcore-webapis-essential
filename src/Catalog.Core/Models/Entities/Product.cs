using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Core.Models.Entities;

[Table("Products")]
public class Product : EntityBase
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; } = default;

    [Required]
    public float Stock { get; set; } = default;

    [Required]
    [Url]
    [StringLength(300)]
    public string ImageUri { get; set; } = string.Empty;

    #region Category 1..* Product
    public int CategoryId { get; set; } = default;

    public Category? Category { get; set; } = null;
    #endregion
}
