using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// Defines the methods and properties that all the configuration nodes that are related
  /// with some type of provider should implements.
  /// </summary>
  public interface IProviderNode
  {
    /// <summary>
    /// Gets the provider name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the assembly-qualified name of the provider type, which includes the name of the assembly
    /// from which the provider type was loaded.
    /// </summary>
    /// <seealso cref="System.Type.AssemblyQualifiedName"/>
    string Type { get; }
  }
}
