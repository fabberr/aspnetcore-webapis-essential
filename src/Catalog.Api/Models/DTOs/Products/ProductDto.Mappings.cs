using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Models.Entities;

namespace Catalog.Api.Models.DTOs.Products;

internal static class ProductDtoMappingExtensions
{
    #region GET
    internal static GetProductResponse[] ToGetResponse(this IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        var convertibleProducts = products.Where(static (product) => product is not null);

        return [.. convertibleProducts.Select(static (product) => (GetProductResponse)product)];
    }

    internal static GetProductResponse ToGetResponse(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return (GetProductResponse)product;
    }
    #endregion

    #region POST
    internal static Product ToEntity(this CreateProductRequest createProductRequest)
    {
        ArgumentNullException.ThrowIfNull(createProductRequest);

        return (Product)createProductRequest;
    }

    internal static CreateProductResponse ToCreateResponse(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return (CreateProductResponse)product;
    }
    #endregion

    #region PUT
    internal static Product ToEntity(this UpdateProductRequest updateProductRequest)
    {
        ArgumentNullException.ThrowIfNull(updateProductRequest);

        return (Product)updateProductRequest;
    }

    internal static UpdateProductResponse ToUpdateResponse(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return (UpdateProductResponse)product;
    }
    #endregion

    #region DELETE
    internal static DeleteProductResponse ToDeleteResponse(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return (DeleteProductResponse)product;
    }
    #endregion
}
