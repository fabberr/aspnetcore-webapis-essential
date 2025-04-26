using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Models.Entities;

/// <summary>
/// Represents the category of a product.<br/>
/// This type cannot be inherited.
/// </summary>
[Table("Categories")]
public sealed partial class Category : EntityBase
{
    /// <summary>
    /// The name of this category.
    /// </summary>
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The URI of this category's thumbnail image.
    /// </summary>
    [Required]
    [StringLength(300)]
    public string ImageUri { get; set; } = string.Empty;
}
