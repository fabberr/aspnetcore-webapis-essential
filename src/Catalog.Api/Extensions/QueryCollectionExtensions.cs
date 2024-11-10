using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Extensions;

internal static class QueryCollectionExtensions
{
    internal static bool TryParseValue<TValue>(
        this IQueryCollection queryCollection,
        string key,
        [NotNullWhen(true)]
        [MaybeNullWhen(false)]
        out TValue result
    )
        where TValue : IParsable<TValue>
    {
        ArgumentNullException.ThrowIfNull(queryCollection);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        result = default;

        string? stringValue;
        if (!queryCollection.TryGetValue(key, out var stringValues) || (stringValue = stringValues.FirstOrDefault()) is null)
        {
            return false;
        }

        if (!TValue.TryParse(stringValue, CultureInfo.InvariantCulture, out var parsedValue))
        {
            return false;
        }

        result = parsedValue;
        return true;
    }
}
