using System;
using System.Configuration;
using System.IO;
using System.Xml;

using Nohros.Resources;

namespace Nohros.Configuration
{
  /// <summary>
  /// A parser for the repository configuration node.
  /// </summary>
  public partial class RepositoryNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="RepositoryNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the repository configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The path of the directory to use as base directory. This directory
    /// should be relative to the application base directory.
    /// </param>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static RepositoryNode Parse(XmlElement element, string base_directory) {
      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string location = GetAttributeValue(element, Strings.kLocationAttribute);
      if (Path.IsPathRooted(location)) {
        throw new ConfigurationErrorsException(
          string.Format(StringResources.Arg_PathRooted, base_directory));
      }

      return new RepositoryNode(name, Path.Combine(base_directory, location));
    }
  }
}