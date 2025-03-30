using System.Net.Mime;
using Catalog.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

/// <summary>
/// A base for Catalog.Api controllers.
/// </summary>
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[TypeFilter<ApiActionLoggingFilter>]
[TypeFilter<ApiExceptionFilter>]
public abstract class CatalogApiController : ControllerBase;
