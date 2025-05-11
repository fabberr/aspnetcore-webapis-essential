using Catalog.Core.Models.Parameters;

namespace Catalog.Api.DTOs;

public partial record PaginationQueryParameters
{
    public static implicit operator PaginationParameters(PaginationQueryParameters source) => PaginationParameters.CreateNew(
        pageNumber: source.PageNumber,
        pageSize: source.PageSize
    );
}
