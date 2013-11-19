using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// Provides a way to replicate the settings defined for a group of
  /// providers.
  /// </summary>
  /// <remarks>
  /// A replica allows the definition of a collection of providers that is
  /// implemented by the same class and has distinct collection of options.
  /// </remarks>
  internal partial class ReplicasNode : AbstractHierarchicalConfigurationNode,
                                      IEnumerable<ReplicaNode>
  {
    readonly string group_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ReplicaNode"/> class by
    /// using the given replica <paramref name="group"/> name.
    /// </summary>
    ReplicasNode(string group) : base(Strings.kReplicasNodeName) {
      group_ = group;
    }
    #endregion

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    /// <inheritdoc/>
    public IEnumerator<ReplicaNode> GetEnumerator() {
      foreach (IConfigurationNode replica in ChildNodes) {
        yield return (ReplicaNode) replica;
      }
    }

    /// <summary>
    /// Gets the name of the group that this node is replicating.
    /// </summary>
    public string Group {
      get { return group_; }
    }
  }
}
