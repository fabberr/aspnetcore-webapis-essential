namespace Catalog.Core.Models.Parameters;

/// <inheritdoc/>
public partial record PaginationParameters
{
    public const int PageNumberMinValue = 1;
    public const int PageNumberMaxValue = int.MaxValue;
    public const int DefaultPageNumber = 1;

    public const int PageSizeMinValue = 1;
    public const int PageSizeMaxValue = 100;
    public const int DefaultPageSize = 10;
}
