using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents the repositories configuration node.
  /// </summary>
  public partial class RepositoriesNode: AbstractHierarchicalConfigurationNode, IRepositoriesNode
  {
    readonly string repositories_base_directory_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoriesNode"/> class
    /// that uses the application base path as the repositories base directory.
    /// </summary>
    public RepositoriesNode() : this(AppDomain.CurrentDomain.BaseDirectory) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoriesNode"/> class
    /// by using the specified <see cref="base_directory"/> as base repository
    /// directory.
    /// </summary>
    /// <param name="base_directory">
    /// The repositories base directory path. This path should be relative
    /// to application base directory.
    /// </param>
    public RepositoriesNode(string base_directory)
      : base(Strings.kRepositoriesNodeName) {
      repositories_base_directory_ = base_directory;
    }
    #endregion

    /// <summary>
    /// Gets a <see cref="RepositoryNode"/> whose name is
    /// <paramref name="repository_name"/>.
    /// </summary>
    /// <param name="repository_name">
    /// The name of the repository.
    /// </param>
    /// <returns>
    /// A <see cref="RepositoryNode"/> object whose name is
    /// <paramref name="repository_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// The <param name="repository_name"> was not found.
    /// </param>
    /// </exception>
    public IRepositoryNode GetRepositoryNode(string repository_name) {
      return this[repository_name] as IRepositoryNode;
    }

    /// <summary>
    /// Gets a <see cref="RepositoryNode"/> whose name is
    /// <paramref name="repository"/>.
    /// </summary>
    /// <param name="repository_name">
    /// The name of the repository.
    /// </param>
    /// <param name="repository">
    /// When this method returns contains a <see cref="RepositoryNode"/> object
    /// whose name is <paramref name="repository_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a repository with name
    /// <paramref name="repository_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    public bool GetRepositoryNode(string repository_name,
      out IRepositoryNode repository) {
      return GetChildNode(repository_name, out repository);
    }

    /// <summary>
    /// Gets the repositories base path.
    /// </summary>
    public string BaseDirectory {
      get { return repositories_base_directory_; }
    }
  }
}
