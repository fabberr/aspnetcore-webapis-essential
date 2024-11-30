using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IQueryCollection"/> instances.
/// </summary>
internal static class QueryCollectionExtensions
{
    /// <summary>
    ///     Attempts to parse a value of type <typeparamref name="TValue"/> from
    ///     the string value of a given <see cref="IQueryCollection"/> entry.
    /// </summary>
    /// <typeparam name="TValue">
    ///     Type of the value to be parsed from the extracted parameter.<br/>
    ///     Must implement <see cref="IParsable{TSelf}"/>.
    /// </typeparam>
    /// <param name="queryCollection">
    ///     The <see cref="IQueryCollection"/> instance.
    /// </param>
    /// <param name="key">
    ///     The name of the query parameter to extract.<br/>
    ///     If the parameter associated with the key contains multiple values,
    ///     only the first one will be extracted.
    /// </param>
    /// <param name="result">
    ///     Output parameter.<br/>
    ///     Set to the parsed value if the parameter is successfully extracted
    ///     and parsed as an instance of <typeparamref name="TValue"/>,
    ///     <see langword="default"/> otherwise.
    /// </param>
    /// <param name="stringValue">
    ///     Output parameter.<br/>
    ///     Set to the string value extracted from the parameter or
    ///     <see langword="null"/> if a parameter associated with the given
    ///     <paramref name="key"/> cannot be found.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> when the parameter is successfully extracted
    ///     and parsed, <see langword="false"/> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="queryCollection"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="key"/> is <see langword="null"/>, empty,
    ///     or consists only of white-space characters
    /// </exception>
    internal static bool TryParseValue<TValue>(
        this IQueryCollection queryCollection,

        string key,

        [NotNullWhen(true)]
        [MaybeNullWhen(false)]
        out TValue result,

        out string? stringValue
    )
        where TValue : IParsable<TValue>
    {
        ArgumentNullException.ThrowIfNull(queryCollection);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        result = default;
        stringValue = null;

        if (!queryCollection.TryGetValue(key, out var stringValues))
        {
            return false;
        }

        stringValue = stringValues.FirstOrDefault();
        if (stringValue is null)
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
