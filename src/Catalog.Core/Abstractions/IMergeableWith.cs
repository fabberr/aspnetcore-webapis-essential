namespace Catalog.Core.Abstractions;

/// <summary>
/// Defines a method for merging two models.
/// </summary>
/// <typeparam name="TModel">
/// Type of the model.
/// </typeparam>
public interface IMergeableWith<TModel>
    where TModel: notnull
{
    /// <summary>
    /// Merges values from this instance with those from the specified
    /// <typeparamref name="TModel"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For each property in this instance, its value will be replaced with the
    /// corresponding value from <paramref name="other"/> only when the
    /// following conditions are met:
    /// </para>
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///     The value from <paramref name="other"/> is present (e.g. not "empty",
    ///     <c><see langword="null"/></c>, or <c><see langword="default"/></c>).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///     The value from <paramref name="other"/> does not match the value
    ///     from this instance.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <param name="other">
    /// The source model to merge with this instance.
    /// </param>
    /// <returns>
    /// This instance, after merging in values from <paramref name="other"/>.
    /// </returns>
    TModel MergeWith(in TModel other);

    /// <summary>
    /// Overwrites values from this instance with those from the specified
    /// <typeparamref name="TModel"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For each property in this instance, its value will be replaced with the
    /// corresponding value from <paramref name="other"/> only when the
    /// following condition is met:
    /// </para>
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///     The value from <paramref name="other"/> is present (e.g. not "empty",
    ///     <c><see langword="null"/></c>, or <c><see langword="default"/></c>).
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <param name="other">
    /// The source model to overwrite this instance with.
    /// </param>
    /// <returns>
    /// This instance, after overwriting values from <paramref name="other"/>.
    /// </returns>
    TModel OverwriteWith(in TModel other);
}
