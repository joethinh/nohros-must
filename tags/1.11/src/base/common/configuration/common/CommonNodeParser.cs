using System;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class CommonNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="CommonNode"/> object using the application base directory
    /// as the configuration base directory.
    /// </summary>
    /// <param name="element">
    /// A  <see cref="XmlElement"/> that contains the common configuration
    /// data.
    /// </param>
    /// <returns>
    /// A <see cref="CommonNode"/> containing the common configuration data.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static CommonNode Parse(XmlElement element) {
      return Parse(element, AppDomain.CurrentDomain.BaseDirectory);
    }

    /// <summary>
    /// Creates a instance of the <see cref="CommonNode"/> object by parsing
    /// the given <paramref name="element"/> Xml element.
    /// </summary>
    /// <param name="element">
    /// A  <see cref="XmlElement"/> that contains the common configuration
    /// data.
    /// </param>
    /// <param name="base_directory">
    /// The path to the directory to use as the base directory when resolving
    /// relative paths.
    /// </param>
    /// <returns>
    /// A <see cref="CommonNode"/> containing the common configuration data.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    /// <remarks>
    /// The <see cref="base_directory"/> is used to resolve the relative paths.
    /// </remarks>
    public static CommonNode Parse(XmlElement element, string base_directory) {
      CommonNode common = new CommonNode();
      foreach(XmlNode node in element) {
        if (node.NodeType == XmlNodeType.Element) {
          string name = node.Name;
          if (StringsAreEquals(name, Strings.kRepositoryNodeName)) {
            common.repositories_ = RepositoriesNode.Parse(element,
              base_directory);
          } else if (StringsAreEquals(name, Strings.kProvidersNodeName)) {
            common.providers_ = ProvidersNode.Parse(element, base_directory);
          }
        }
      }
      return common;
    }
  }
}
