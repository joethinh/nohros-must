using System;

using Nohros.Resources;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents a chain of objects that is a ordered list of objects.
  /// </summary>
  public class ChainNode: AbstractConfigurationNode
  {
    string[] nodes_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ChainNode"/> class by
    /// using the specified chain name.
    /// </summary>
    /// <param name="name"></param>
    public ChainNode(string name) : base(name) { }
    #endregion

    /// <summary>
    /// Gets the nodes that compose the chain.
    /// </summary>
    public string[] Nodes {
      get { return nodes_; }
    }
  }
}
