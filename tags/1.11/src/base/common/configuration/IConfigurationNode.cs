using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents a single node in the nohros configuration file.
  /// </summary>
  public interface IConfigurationNode
  {
    /// <summary>
    /// Gets the name of the node.
    /// </summary>
    string Name { get; }
  }
}