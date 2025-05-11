using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Catalog.Core.Models.Parameters;

namespace Catalog.Core.Abstractions.Factories;

/// <summary>
/// Defines static methods for creating instances of
/// <see cref="PaginationParameters"/>.
/// </summary>
public interface IPaginationParametersFactory
{
    #region Properties
    /// <summary>
    /// Gets an instance of <see cref="PaginationParameters"/> with all
    /// properties set to default values.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term><c>PageNumber</c></term>
    ///         <description><see cref="PageNumberDefaultValue"/></description>
    ///     </item>
    ///     <item>
    ///         <term><c>PageSize</c></term>
    ///         <description><see cref="PageSizeDefaultValue"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    abstract static PaginationParameters Default { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Attempts to create a new instance of <see cref="PaginationParameters"/>
    /// from a given set of parameters.
    /// </summary>
    /// <remarks>
    /// This method only catches instances of <see cref="ValidationException"/>.
    /// </remarks>
    /// <param name="result">
    /// The instance of <see cref="PaginationParameters"/> that was created.<br/>
    /// When this method returs <see langword="true"/>, is guaranteed to be in a
    /// valid state.
    /// </param>
    /// <param name="originalException">
    /// The original <see cref="ValidationException"/> instance that was caught.
    /// </param>
    /// <param name="pageNumber">
    /// The Page number.<br/>
    /// Must be within the range:
    /// [<see cref="PageNumberMinValue"/>, <see cref="PageNumberMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageNumberDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <param name="pageSize">
    /// Number of items to fetch from the data source.<br/>
    /// Must be within the range:
    /// [<see cref="PageSizeMinValue"/>, <see cref="PageSizeMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageSizeDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if it was possible to create a new instance of
    /// <see cref="PaginationParameters"/> from the supplied parameters,
    /// <see langword="false"/> otherwise.
    /// </returns>
    abstract static bool TryCreateNew(
        [NotNullWhen(returnValue: true)]
        out PaginationParameters? result,

        [NotNullWhen(returnValue: false)]
        out ValidationException? originalException,

        int? pageNumber = null,

        int? pageSize = null
    );

    /// <summary>
    /// Attempts to create a new instance of <see cref="PaginationParameters"/>
    /// from a given set of parameters.
    /// </summary>
    /// <remarks>
    /// This method does not throw exceptions.
    /// </remarks>
    /// <param name="result">
    /// The instance of <see cref="PaginationParameters"/> that was created.<br/>
    /// When this method returs <see langword="true"/>, is guaranteed to be in a
    /// valid state.
    /// </param>
    /// <param name="pageNumber">
    /// The Page number.<br/>
    /// Must be within the range:
    /// [<see cref="PageNumberMinValue"/>, <see cref="PageNumberMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageNumberDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <param name="pageSize">
    /// Number of items to fetch from the data source.<br/>
    /// Must be within the range:
    /// [<see cref="PageSizeMinValue"/>, <see cref="PageSizeMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageSizeDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if it was possible to create a new instance of
    /// <see cref="PaginationParameters"/> from the supplied parameters,
    /// <see langword="false"/> otherwise.
    /// </returns>
    abstract static bool TryCreateNew(
        [NotNullWhen(returnValue: true)]
        out PaginationParameters? result,

        int? pageNumber = null,

        int? pageSize = null
    );

    /// <summary>
    /// Creates a new instance of <see cref="PaginationParameters"/> from a
    /// given set of parameters.
    /// </summary>
    /// <remarks>
    /// This method ensures the provided values are valid for this type's
    /// invariant.<br/>
    /// <br/>
    /// If any of the values are not considered valid, a
    /// <see cref="ValidationException"/> is thrown.
    /// </remarks>
    /// <param name="pageNumber">
    /// The Page number.<br/>
    /// Must be within the range:
    /// [<see cref="PageNumberMinValue"/>, <see cref="PageNumberMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageNumberDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <param name="pageSize">
    /// Number of items to fetch from the data source.<br/>
    /// Must be within the range:
    /// [<see cref="PageSizeMinValue"/>, <see cref="PageSizeMaxValue"/>].<br/>
    /// <br/>
    /// When not provided, <see cref="PageSizeDefaultValue"/> will be used to
    /// create the instance.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="PaginationParameters"/>. The instance
    /// returned by this method is guaranteed to be in a valid state.
    /// </returns>
    abstract static PaginationParameters CreateNew(
        int? pageNumber = null,
        int? pageSize = null
    );
    #endregion
}
