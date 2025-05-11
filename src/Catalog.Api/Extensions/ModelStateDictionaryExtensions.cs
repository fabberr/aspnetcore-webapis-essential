using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Catalog.Api.Extensions;

/// <summary>
/// Extensions for manipulating <see cref="ModelStateDictionary"/> instances.
/// </summary>
public static class ModelStateDictionaryExtensions
{
    /// <summary>
    /// Converts this instance of to a new <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="dictionary">
    /// Instance to convert.
    /// </param>
    /// <returns>
    /// The new <see cref="ModelStateDictionary"/> instance that was created.
    /// </returns>
    public static ModelStateDictionary ToModelState(this IDictionary<string, IEnumerable<string>> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        var modelState = new ModelStateDictionary();

        foreach (var (key, errorMessages) in dictionary)
        {
            foreach (var message in errorMessages)
            {
                modelState.TryAddModelError(key, message);
            }
        }

        return modelState;
    }

    /// <summary>
    /// Copies key/value pairs from this instance to an existing
    /// <see cref="ModelStateDictionary"/> instance.
    /// </summary>
    /// <param name="dictionary">
    /// Instance to copy values from.
    /// </param>
    /// <param name="modelState">
    /// Instance to copy values to.
    /// </param>
    public static void CopyTo(this IDictionary<string, IEnumerable<string>> dictionary, ModelStateDictionary modelState)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(modelState);

        foreach (var (key, errorMessages) in dictionary)
        {
            foreach (var message in errorMessages)
            {
                modelState.TryAddModelError(key, message);
            }
        }
    }
}
