using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ApiController]
public abstract class CatalogApiController : ControllerBase;
