using System.ComponentModel.DataAnnotations;
using Catalog.Core.Models.Parameters;

namespace Catalog.Api.DTOs.Pagination;

public partial record PaginationQueryParameters(
    [Range(minimum: PaginationParameters.PageNumberMinValue, maximum: PaginationParameters.PageNumberMaxValue)]
    int? PageNumber = null,

    [Range(minimum: PaginationParameters.PageSizeMinValue, maximum: PaginationParameters.PageSizeMaxValue)]
    int? PageSize = null
);
