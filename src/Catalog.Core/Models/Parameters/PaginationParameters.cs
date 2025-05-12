using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Core.Models.Parameters;

/// <summary>
/// Represents a set of parameters for fetching paginated entries from the data
/// source.
/// </summary>
public partial record PaginationParameters
{
    [Required]
    [Range(minimum: PageNumberMinValue, maximum: PageNumberMaxValue)]
    public int PageNumber { get; private init; }

    [Required]
    [Range(minimum: PageSizeMinValue, maximum: PageSizeMaxValue)]
    public int PageSize { get; private init; }

    /**
     * Typically, this method would be generated automatically by the compiler
     * for `record` types, however this is only the case when we provide a
     * primary constructor, which are always public.
     *
     * This type implements a factory pattern to ensure its class invariant is
     * not violated after instantiation so we can't actually provide any public
     * constructors and thus, the `Deconstruct` method has to be implemened
     * manually if we want this functionality.
    */
    public void Deconstruct(out int pageNumber, out int pageSize)
    {
        pageNumber = PageNumber;
        pageSize = PageSize;
    }
}
