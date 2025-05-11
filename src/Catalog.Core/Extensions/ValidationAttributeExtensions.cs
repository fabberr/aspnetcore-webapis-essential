using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Catalog.Core.Extensions;

/// <summary>
/// Extensions for <see cref="ValidationAttribute"/>.
/// </summary>
public static class ValidationAttributeExtensions
{
    /// <summary>
    /// Attempts to validate the specified property.
    /// </summary>
    /// <param name="attribute">
    /// <see cref="ValidationAttribute"/> instance.
    /// </param>
    /// <param name="property">
    /// Property to validate.
    /// </param>
    /// <param name="context">
    /// The <see cref="ValidationContext"/> object that describes the context
    /// where the validation checks are performed.
    /// </param>
    /// <param name="result">
    /// Validation result. If the validation suceeds, this parameter will be set
    /// to <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the specified property passed validation,
    /// <see langword="false"/> otherwise.
    /// </returns>
    public static bool TryValidateProperty(
        this ValidationAttribute attribute,
        
        PropertyInfo property,
        ValidationContext context,
        
        [NotNullWhen(false)]
        out ValidationResult? result
    )
    {
        ArgumentNullException.ThrowIfNull(attribute);
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(context);

        result = null;
        try
        {
            attribute.Validate(
                value: property.GetValue(context.ObjectInstance),
                validationContext: context
            );

            return true;
        }
        catch (ValidationException ex)
        {
            result = ex.ValidationResult;
            return false;
        }
    }
}
