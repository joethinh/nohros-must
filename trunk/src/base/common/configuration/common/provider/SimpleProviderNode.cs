using System;

namespace Nohros.Configuration
{
  /// <summary>
  /// <see cref="SimpleProviderNode"/> is the default implementation of the
  /// <see cref="ISimpleProviderNode"/> interface.
  /// </summary>
  public partial class SimpleProviderNode: ProviderNode, ISimpleProviderNode
  {
    string group_;

    #region .ctor
    /// <summary>
    /// Intializes a new instance of the <see cref="SimpleProviderNode"/> by
    /// using the specified provider name and type.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies the provider within a collection.
    /// </param>
    /// <param name="type">
    /// The assembly's qualified name of the provider type.</param>
    public SimpleProviderNode(string name, string type)
      : this(name, type, string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleProviderNode"/> by
    /// using the specified provider name, type and related group.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public SimpleProviderNode(string name, string type, string group)
      : base(name, type) {
      if (group == null) {
        throw new ArgumentNullException("group");
      }
      group_ = group;
    }
    #endregion

    /// <summary>
    /// Gets the group name of the provider.
    /// </summary>
    /// <value>
    /// The group name of the provider as was specified in configuration node.
    /// If the group name was not supplied in the configuration node, this
    /// method will return a empty string.
    /// </value>
    /// <remarks>
    /// The group property is used to group providers that have the same
    /// behavior or is similar.
    /// </remarks>
    public string Group {
      get { return group_; }
    }
  }
}
