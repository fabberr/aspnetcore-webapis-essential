using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Catalog.Core.Exceptions;
using Catalog.Core.Extensions;

namespace Catalog.Core.Models.Parameters;

/// <inheritdoc/>
public partial record PaginationParameters
    : IValidatableObject
{
    #region Private
    private static PropertyInfo[] _publicNonStaticProperties = typeof(PaginationParameters)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

    private PaginationParameters EnsureValidInternalState()
    {
        var context = new ValidationContext(this);
        var results = Validate(context).ToArray();

        if (results.Length == 0)
        {
            return this;
        }

        var errors = results
            .SelectMany(result => result.MemberNames.Select(name => (Name: name, Message: result.ErrorMessage!)))
            .GroupBy(tuple => tuple.Name, tuple => tuple.Message)
            .ToDictionary(group => group.Key, group => group.Distinct());

        throw new ModelValidationException(errors);
    }
    #endregion

    #region IValidatableObject
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var property in _publicNonStaticProperties)
        {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>(inherit: true);

            foreach (var attribute in validationAttributes)
            {
                var context = new ValidationContext(instance: this) {
                    MemberName = property.Name,
                };

                if (!attribute.TryValidateProperty(property, context, out var result))
                {
                    yield return new ValidationResult(
                        errorMessage: result.ErrorMessage,
                        memberNames: [property.Name]
                    );
                }
            }
        }
    }
    #endregion
}
