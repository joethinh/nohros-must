using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProviderNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="ProviderNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the provider configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the providers's location.
    /// </param>
    /// <returns>
    /// A <see cref="ProviderNode"/> containing the configured provider.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static ProviderNode Parse(XmlElement element, string base_directory,
      out IList<string> options_refs) {
      CheckPreconditions(element, base_directory);
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kTypeAttribute);
      string location = GetLocation(element, base_directory);
      string group = GetAttributeValue(element, Strings.kGroupAttribute,
        string.Empty);

      ProviderNode provider = new ProviderNode(name, type);
      provider.location_ = location;
      provider.group_ = group;
      provider.options = GetOptions(name, element, out options_refs);
      return provider;
    }

    protected static ProviderOptionsNode GetOptions(string name,
      XmlElement element,
      out IList<string> options_references) {
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kOptionsNodeName)) {
          return ProviderOptionsNode
            .Parse(name, (XmlElement) node, out options_references);
        }
      }
      options_references = new List<string>();
      return new ProviderOptionsNode(name);
    }
  }
}
