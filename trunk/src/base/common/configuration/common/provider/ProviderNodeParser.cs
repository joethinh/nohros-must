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
    public static ProviderNode Parse(XmlElement element, string base_directory) {
      CheckPreconditions(element, base_directory);
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kTypeAttribute);
      string location = GetLocation(element, base_directory);
      string group = GetAttributeValue(element, Strings.kGroupAttribute,
        string.Empty);

      ProviderNode provider = new ProviderNode(name, type);
      provider.location_ = location;
      provider.group_ = group;
      provider.options = GetOptions(element);
      return provider;
    }

    /// <summary>
    /// Gets the provider options.
    /// </summary>
    /// <param name="element">
    /// A <see cref="XmlElement"/> the contains data about the provider to get
    /// the options for.
    /// </param>
    /// <returns>
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the
    /// options configured for the provider.
    /// </returns>
    /// <remarks>
    /// If no options was configured for the given provider, this method will
    /// returns a empty <see cref="IDictionary{TKey,TValue}"/>.
    /// </remarks>
    protected static IDictionary<string, string> GetOptions(XmlElement element) {
      IDictionary<string, string> options = new Dictionary<string, string>();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          StringsAreEquals(node.Name, Strings.kOptionsNodeName)) {
          // at this point we know for sure that the node is a XmlElement, so
          // [node.Attributes] will alwasy not null.
          foreach (XmlAttribute attribute in node.Attributes) {
            options[attribute.Name] = attribute.Value;
          }
        }
      }
      return options;
    }
  }
}
