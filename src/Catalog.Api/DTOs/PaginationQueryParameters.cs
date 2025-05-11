namespace Catalog.Api.DTOs;

public partial record PaginationQueryParameters(
    // [Range(minimum: PaginationParameters.PageNumberMinValue, maximum: PaginationParameters.PageNumberMaxValue,
    //     ErrorMessage = "The value must be greater than or equal to {1}."
    // )]
    int? PageNumber = null,

    // [Range(minimum: PaginationParameters.PageSizeMinValue, maximum: PaginationParameters.PageSizeMaxValue)]
    int? PageSize = null
);
