using System;
using Catalog.Core.Models.Abstractions;

namespace Catalog.Core.Models.Entities;

public sealed partial class Product
    : IMergeableWith<Product>
{
    #region IMergeableWith<Product>
    /// <inheritdoc/>
    public Product MergeWith(in Product other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.Name is not null && other.Name != this.Name)
            this.Name = new(other.Name.AsSpan());

        if (other.Description is not null && other.Description != this.Description)
            this.Description = new(other.Description.AsSpan());

        if (other.Price is not 0 && other.Price != this.Price)
            this.Price = other.Price;

        if (other.Stock is not 0 && other.Stock != this.Stock)
            this.Stock = other.Stock;

        if (other.ImageUri is not null && other.ImageUri != this.ImageUri)
            this.ImageUri = new(other.ImageUri.AsSpan());

        if (other.CategoryId is not 0 && other.CategoryId != this.CategoryId)
            this.CategoryId = other.CategoryId;

        return this;
    }

    /// <inheritdoc/>
    public Product OverwriteWith(in Product other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.Name is not null)
            this.Name = new(other.Name.AsSpan());

        if (other.Description is not null)
            this.Description = new(other.Description.AsSpan());

        if (other.Price is not 0)
            this.Price = other.Price;

        if (other.Stock is not 0)
            this.Stock = other.Stock;

        if (other.ImageUri is not null)
            this.ImageUri = new(other.ImageUri.AsSpan());

        if (other.CategoryId is not 0)
            this.CategoryId = other.CategoryId;

        return this;
    }
    #endregion
}
