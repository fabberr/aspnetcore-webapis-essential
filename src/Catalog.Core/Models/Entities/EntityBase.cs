using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models.Entities;

/// <summary>
/// Represents the base entity, defining common properties shared by all entities.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// The unique identifier of this entity.
    /// </summary>
    [Key]
    [Editable(false)]
    public int Id { get; init; } = default;
    
    /// <summary>
    /// The timestamp of when this entity was first created (UTC).<br/>
    /// This value is read-only.
    /// </summary>
    [Required]
    [Editable(false)]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// The timestamp of when this entity was last modified (UTC).<br/>
    /// This property is set automatically by the <see cref="Context.CatalogDbContext"/>
    /// for all modified entities before changes are commited to the database.
    /// </summary>
    public DateTime? UpdatedAt { get; set; } = null;

    /// <summary>
    /// Indicates whether this entity is hidden or not.<br/>
    /// <see langword="true"/> when the entity is hidden, <see langword="false"/> otherwise.
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool Hidden { get; set; } = default;
}
