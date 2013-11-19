using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// Defines a collection of options that should be replaced in a replica.
  /// </summary>
  internal partial class ReplicaNode : AbstractConfigurationNode
  {
    readonly IProviderOptions options_;

    #region .ctor
    ReplicaNode(string name, IProviderOptions options):base(name) {
      options_ = options;
    }
    #endregion

    /// <summary>
    /// Gets the collection of options that should be added/replaced in a
    /// replica.
    /// </summary>
    public IProviderOptions Options {
      get { return options_; }
    }
  }
}
