using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="IRepositoriesNode"/> is a collection of
  /// <see cref="IRepositoryNode"/>.
  /// </summary>
  public interface IRepositoriesNode : IConfigurationNode
  {
    /// <summary>
    /// Gets a <see cref="IRepositoriesNode"/> whose name is
    /// <paramref name="repository_name"/>.
    /// </summary>
    /// <param name="repository_name">
    /// The name of the repository.
    /// </param>
    /// <returns>
    /// A <see cref="IRepositoriesNode"/> object whose name is
    /// <paramref name="repository_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// The <param name="repository_name"> was not found.
    /// </param>
    /// </exception>
    IRepositoryNode GetRepositoryNode(string repository_name);

    /// <summary>
    /// Gets a <see cref="IRepositoriesNode"/> whose name is
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
    bool GetRepositoryNode(string repository_name,
      out IRepositoryNode repository);

    /// <summary>
    /// Gets the repositories base path.
    /// </summary>
    string BaseDirectory { get; }
  }
}
