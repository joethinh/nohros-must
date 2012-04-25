using System;
using System.IO;
using System.Xml;
using Nohros.Resources;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="RepositoryNode"/> is a container for pathnames. It is used
  /// to resolve relative pathnames to absolute pathnames using the application
  /// base directory. This allow applications to configure relative paths and
  /// resolve it to its associated full path.
  /// </summary>
  public class RepositoryNode : AbstractConfigurationNode
  {
    string relative_path_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryNode"/> class by
    /// using the specified repository name.
    /// </summary>
    /// <param name="name">
    /// The name of the repository.
    /// </param>
    /// <remarks>
    /// By using this constructor the path of the repository will be set to
    /// the application base directory.
    /// </remarks>
    public RepositoryNode(string name)
      : base(name) {
      relative_path_ = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the RepositoryNode by using the specified
    /// repository name and path.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the repository within an application.
    /// </param>
    /// <param name="relative_path">
    /// The application relative path of the repository.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> or <paramref name="relative_path"/> is
    /// a null reference.
    /// </exception>
    /// <remarks>
    /// The existence of the physical path is not checked.
    /// </remarks>
    public RepositoryNode(string name, string relative_path)
      : base(name) {
      if (relative_path == null)
        throw new ArgumentNullException("relative_path");
      RelativePath = relative_path;
    }
    #endregion

    /// <summary>
    /// Parses a Xml node that contains information about a repository.
    /// </summary>
    /// <param name="node">
    /// A Xml node containing the data to parse.
    /// </param>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="node"/> is not a valid representation of a
    /// repository or the repository node contains an rooted path.
    /// </exception>
    public void Parse(XmlNode node) {
      string relative_path;
      if (!GetAttributeValue(node, "relative-path", out relative_path))
        throw new ConfigurationException(
          string.Format(StringResources.Config_ErrorAt, "attribute name",
            NohrosConfiguration.kRepositoryNodeTree + "." + name));

      RelativePath = relative_path;
    }

    /// <summary>
    /// Gets or sets the path pointed by the repository.
    /// </summary>
    /// <remarks>
    /// Attempt to get the value of this property return the fully qualified
    /// path of the repository. While setting the value of this property the
    /// caller must specifies a non-rooted path that is relative to the
    /// application base directory.
    /// </remarks>
    public string RelativePath {
      get { return relative_path_; }
      set {
        // sanity check if the path is rooted. It must be relative to the
        // application base directory.
        if (Path.IsPathRooted(value))
          throw new ArgumentOutOfRangeException(
            string.Format(StringResources.Config_PathIsRooted, value));

        relative_path_ = value;
      }
    }

    /// <summary>
    /// Get the full path pointed by the repository.
    /// </summary>
    public string FullPath {
      get {
        return Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory, relative_path_);
      }
    }
  }
}
