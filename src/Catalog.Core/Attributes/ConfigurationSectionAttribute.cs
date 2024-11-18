using System;

namespace Catalog.Core.Attributes;

/// <summary>
/// Specifies that a type maps an ASP.NET Core configuration section.
/// This class cannot be inherited.
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class ConfigurationSectionAttribute : Attribute
{
    private readonly string _sectionName;

    /// <summary>
    /// Initialized a new instance of the <see cref="ConfigurationSectionAttribute"/>
    /// class with the specified section name.
    /// </summary>
    /// <param name="sectionName">
    /// Name of the configuration section.
    /// </param>
    public ConfigurationSectionAttribute(string sectionName) => _sectionName = sectionName;

    /// <summary>
    /// The configuration section name.
    /// </summary>
    public string SectionName => _sectionName;
}
