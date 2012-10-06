using System;

namespace Nohros
{
  /// <summary>
  /// Defines a <see cref="Type"/> that is dynamically loaded at runtime.
  /// </summary>
  public interface IRuntimeType
  {
    /// <summary>
    /// Gets the assembly-qualified name of the provider type, which includes
    /// the name of the assembly from which the provider type was loaded.
    /// </summary>
    /// <seealso cref="System.Type.AssemblyQualifiedName"/>
    string Type { get; }

    /// <summary>
    /// Gets a string representing the fully qualified path to the directory
    /// where the assembly associated with the provider is located.
    /// </summary>
    /// <value>
    /// The fully qualified path to the folder where the provider assembly is
    /// stored.
    /// </value>
    /// <remarks>
    /// The assembly location must be an absolute path or a path relative to
    /// the configuration file.
    /// <para>
    /// The default location returned by this property is the application base
    /// directory.
    /// </para>
    /// </remarks>
    string Location { get; }
  }
}
