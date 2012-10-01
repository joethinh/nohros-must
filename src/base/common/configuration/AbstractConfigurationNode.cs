using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// Provides a skeletal implementation of the
  /// <see cref="IConfigurationNode"/> interface to reduce the effort required
  /// to implement this interface.
  /// </summary>
  public abstract partial class AbstractConfigurationNode : IConfigurationNode
  {
    /// <summary>
    /// The name of the configuration node.
    /// </summary>
    protected string name;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AbstractConfigurationNode"/>
    /// </summary>
    /// <param name="name">The name of the node.</param>
    protected AbstractConfigurationNode(string name) {
#if DEBUG
      if (name == null) {
        throw new ArgumentNullException("name");
      }
#endif
      this.name = name;
    }
    #endregion

    /// <summary>
    /// Gets the name of the node.
    /// </summary>
    public string Name {
      get { return name; }
      internal set { name = value; }
    }
  }
}
