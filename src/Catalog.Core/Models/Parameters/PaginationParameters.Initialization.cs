namespace Catalog.Core.Models.Parameters;

/// <inheritdoc/>
public partial record PaginationParameters
{
    protected PaginationParameters()
    {
        PageNumber = DefaultPageNumber;
        PageSize = DefaultPageSize;
    }

    protected PaginationParameters(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
