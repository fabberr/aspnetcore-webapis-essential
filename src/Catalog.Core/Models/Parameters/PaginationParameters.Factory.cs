using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Catalog.Core.Abstractions.Factories;

namespace Catalog.Core.Models.Parameters;

/// <inheritdoc/>
public partial record PaginationParameters
    : IPaginationParametersFactory
{
    #region IPaginationParametersFactory::Properties
    public static PaginationParameters Default => new();
    #endregion

    #region IPaginationParametersFactory::Methods
    private static bool TryCreateNewImpl<TException>(
        [NotNullWhen(returnValue: true)]
        out PaginationParameters? result,

        [NotNullWhen(returnValue: false)]
        out TException? originalException,

        int? pageNumber = null,
        int? pageSize = null
    )
        where TException : Exception
    {
        (originalException, result) = (null, null);
        try
        {
            result = CreateNew(
                pageNumber: pageNumber,
                pageSize: pageSize
            );
            return true;
        }
        catch (TException ex)
        {
            originalException = ex;
            return false;
        }
    }

    public static bool TryCreateNew(
        [NotNullWhen(returnValue: true)]
        out PaginationParameters? result,

        [NotNullWhen(returnValue: false)]
        out ValidationException? originalException,

        int? pageNumber = null,
        int? pageSize = null
    ) => TryCreateNewImpl(
        result: out result,
        originalException: out originalException,
        pageNumber: pageNumber,
        pageSize: pageSize
    );

    public static bool TryCreateNew(
        [NotNullWhen(returnValue: true)]
        out PaginationParameters? result,

        int? pageNumber = null,
        int? pageSize = null
    ) => TryCreateNewImpl<Exception>(
        result: out result,
        originalException: out _,
        pageNumber: pageNumber,
        pageSize: pageSize
    );

    public static PaginationParameters CreateNew(
        int? pageNumber = null,
        int? pageSize = null
    ) => new PaginationParameters(
        pageNumber: pageNumber ?? DefaultPageNumber,
        pageSize: pageSize ?? DefaultPageSize
    ).EnsureValidInternalState();
    #endregion
}
