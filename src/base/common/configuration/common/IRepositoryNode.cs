using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="RepositoryNode"/> is a container for pathnames. It is used
  /// to resolve relative pathnames to absolute pathnames using the application
  /// base directory. This allows applications to associate constants to
  /// configurable pathnames.
  /// </summary>
  public interface IRepositoryNode : IConfigurationNode
  {
    /// <summary>
    /// Gets or sets the path pointed by the repository.
    /// </summary>
    string RelativeDirectory { get; }

    /// <summary>
    /// Get the full path pointed by the repository.
    /// </summary>
    string AbsoluteDirectory { get; }
  }
}
