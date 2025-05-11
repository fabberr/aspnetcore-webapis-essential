using System;
using System.Collections.Generic;

namespace Catalog.Core.Exceptions;

public class ModelValidationException(
    IDictionary<string, IEnumerable<string>> validationErrors,
    string? message = null
)
    : Exception(message)
{
    public IDictionary<string, IEnumerable<string>> ModelState { get; } = validationErrors;
}
