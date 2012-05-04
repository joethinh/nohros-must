using System;
using System.Collections.Generic;
using System.Text;

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
  public interface ISimpleProviderNode : IProviderNode
  {
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
    string Group { get; }
  }
}
