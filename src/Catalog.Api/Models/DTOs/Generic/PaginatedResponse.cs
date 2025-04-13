using System.Collections.Generic;

namespace Catalog.Api.Models.DTOs.Generic;

internal sealed record class PaginatedResponse<T>(
    uint ItemsPerPage,
    uint Count,
    List<T> Items
) where T : class;
