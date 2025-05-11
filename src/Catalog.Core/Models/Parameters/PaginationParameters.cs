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
}
