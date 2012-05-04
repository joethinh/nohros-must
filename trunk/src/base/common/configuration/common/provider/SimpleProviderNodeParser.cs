using System;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class SimpleProviderNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="CacheProviderNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the repositories configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the providers's location.
    /// </param>
    /// <returns>
    /// A <see cref="CacheProviderNode"/> containing the configured cache
    /// providers.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static SimpleProviderNode Parse(XmlElement element,
      string base_directory) {
      CheckPreconditions(element, base_directory);
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kTypeAttribute);
      string group = GetAttributeValue(element, Strings.kNameAttribute,
        string.Empty);
      string alias = GetAttributeValue(element, Strings.kAliasAttribute,
        string.Empty);
      return new SimpleProviderNode(name, alias, type, group);
    }
  }
}
