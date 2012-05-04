using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class ProviderNode
  {
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
