using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class DataProviderNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="DataProviderNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the data provider configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the repository's location.
    /// </param>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// <paramref name="base_directory"/> does not exists.
    /// </exception>
    public static DataProviderNode Parse(XmlElement element,
      string base_directory) {
      CheckPreconditions(element, base_directory);

      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kTypeAttribute);
      string location = GetLocation(element, base_directory);
      string alias = GetAttributeValue(element, Strings.kAliasAttribute,
        string.Empty);

      DataProviderNode node = new DataProviderNode(name, alias, type, location);
      node.options = GetOptions(element);
      return node;
    }
  }
}
