using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Models.Entities;

[Table("Categories")]
public class Category : EntityBase
{
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Url]
    [StringLength(300)]
    public string ImageUri { get; set; } = string.Empty;

    #region Category 1..* Product
    public ICollection<Product>? Products { get; set; } = [];
    #endregion
}
