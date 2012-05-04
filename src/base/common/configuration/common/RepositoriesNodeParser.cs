using System;
using System.IO;
using System.Xml;
using Nohros.Resources;

namespace Nohros.Configuration
{
  public partial class RepositoriesNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="RepositoriesNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the repositories configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the repository's location.
    /// </param>
    /// <returns>
    /// A <see cref="RepositoriesNode"/> containing the configured repositories.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static RepositoriesNode Parse(XmlElement element,
      string base_directory) {
      CheckPreconditions(element, base_directory);

      string location = GetLocation(element, base_directory);
      RepositoriesNode repositories = new RepositoriesNode(location);
      foreach(XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          StringsAreEquals(node.Name, Strings.kRepositoryNodeName)) {
          RepositoryNode repository = RepositoryNode.Parse(
            (XmlElement) node, location);
          repositories.AddChildNode(repository);
        }
      }
      return repositories;
    }
  }
}
