using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
  /// <summary>
  /// A simple non abstract implementation of the <see cref="IProviderNode"/>
  /// interface.
  /// </summary>
  /// <remarks>
  /// This class defines a common and simple way to define types in
  /// configuration files to dynamic load it at run-time.
  /// </remarks>
  public class SimpleProviderNode : ProviderNode
  {
    const string kGroupKey = "group";

    string group_;

    /// <summary>
    /// Intializes a new instance of the <see cref="SimpleProviderNode"/> by
    /// using the specified provider name and type.
    /// </summary>
    /// <param name="name">The name of the provider.</param>
    /// <param name="type">The assembly's qualified name of the provider
    /// type.</param>
    public SimpleProviderNode(string name, string type): base(name, type) {
      group_ = string.Empty;
    }

    /// <inheritdoc/>
    public override void Parse(XmlNode node, string base_path) {
      InternalParse(node, base_path);

      if (!GetAttributeValue(node, kGroupKey, out group_))
        group_ = string.Empty;
    }

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
