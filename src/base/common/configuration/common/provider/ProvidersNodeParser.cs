using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProvidersNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="ProvidersNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the providers configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the providers's location.
    /// </param>
    /// <returns>
    /// A <see cref="ProvidersNode"/> containing the configured providers.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static ProvidersNode Parse(XmlElement element, string base_directory) {
      CheckPreconditions(element, base_directory);
      ProvidersNode providers = new ProvidersNode();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element) {
          if (StringsAreEquals(node.Name, Strings.kDataProvidersNodeName)) {
            providers.data_provider_node_ = DataProvidersNode.Parse(
              element, base_directory);
          } else if (StringsAreEquals(node.Name, Strings.kCacheProvidersNodeName)) {
            providers.cache_providers_node_ = CacheProvidersNode.Parse(
              element, base_directory);
          } else if (StringsAreEquals(node.Name, Strings.kSimpleProvidersNodeName)) {
            providers.simple_providers_node_ = SimpleProvidersNode.Parse(
              element, base_directory);
          }
        }
      }
      return providers;
    }
  }
}
