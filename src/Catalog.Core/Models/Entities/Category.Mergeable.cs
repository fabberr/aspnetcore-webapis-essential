using System;
using Catalog.Core.Abstractions;

namespace Catalog.Core.Models.Entities;

public sealed partial class Category
    : IMergeableWith<Category>
{
    #region IMergeableWith<Category>
    /// <inheritdoc/>
    public Category MergeWith(in Category other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.Name is not null && other.Name != this.Name)
            this.Name = new(other.Name.AsSpan());

        if (other.ImageUri is not null && other.ImageUri != this.ImageUri)
            this.ImageUri = new(other.ImageUri.AsSpan());

        return this;
    }

    /// <inheritdoc/>
    public Category OverwriteWith(in Category other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.Name is not null && other.Name != this.Name)
            this.Name = new(other.Name.AsSpan());

        if (other.ImageUri is not null && other.ImageUri != this.ImageUri)
            this.ImageUri = new(other.ImageUri.AsSpan());

        return this;
    }
    #endregion
}
