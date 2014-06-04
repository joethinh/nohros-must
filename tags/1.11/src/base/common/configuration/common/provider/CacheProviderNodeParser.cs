using System;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class CacheProviderNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="CacheProviderNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the providers configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the provider's location.
    /// </param>
    /// <returns>
    /// A <see cref="CacheProviderNode"/> containing the configured providers.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public CacheProviderNode Parse(XmlElement element, string base_directory) {
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kNameAttribute);
      string location = GetLocation(element, Strings.kNameAttribute);
      string alias = GetAttributeValue(element, Strings.kAliasAttribute,
        string.Empty);
      return new CacheProviderNode(name, alias, type, location);
    }
  }
}
