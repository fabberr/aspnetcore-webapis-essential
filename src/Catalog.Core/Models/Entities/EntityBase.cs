using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models.Entities;

public abstract class EntityBase
{
    [Key]
    [Editable(false)]
    public int Id { get; init; } = default;

    [Required]
    [Editable(false)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = null;

    [Required]
    [DefaultValue(false)]
    public bool Hidden { get; set; } = default;
}
