using System;
using System.IO;

using Nohros.Resources;

namespace Nohros.Configuration
{
  /// <summary>
  /// Provides a basic implementation of the <see cref="IRepositoriesNode"/>
  /// interface.
  /// </summary>
  public partial class RepositoryNode : AbstractConfigurationNode, IRepositoryNode
  {
    readonly string repository_directory_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryNode"/> class by
    /// using the specified repository name.
    /// </summary>
    /// <param name="name">
    /// The name of the repository.
    /// </param>
    /// <remarks>
    /// By using this constructor the path of the repository will be resolved
    /// to the configured repositories base path, that is the application base
    /// directory by default.
    /// </remarks>
    public RepositoryNode(string name) : base(name) {
      repository_directory_ = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryNode"/> by
    /// using the specified repository name and path.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the repository within an application.
    /// </param>
    /// <param name="path">
    /// The repository's directory. This value should be relative to the
    /// configured repositories base path, that is the application base
    /// directory by default.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> or <paramref name="path"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The physical existence of the path is not checked.
    /// </remarks>
    public RepositoryNode(string name, string path) : base(name) {
      if (path == null) {
        throw new ArgumentNullException("path");
      }
      repository_directory_ = path;
    }
    #endregion

    /// <inheritdoc/>
    public string RelativeDirectory {
      get { return repository_directory_; }
    }

    /// <inheritdoc/>
    public string AbsoluteDirectory {
      get {
        return Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory, repository_directory_);
      }
    }
  }
}
