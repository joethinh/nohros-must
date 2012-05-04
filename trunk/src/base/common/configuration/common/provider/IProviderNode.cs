using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// Defines the methods and properties that all the configuration nodes that
  /// are related with some type of provider should implements.
  /// </summary>
  public interface IProviderNode : IConfigurationNode
  {
    /// <summary>
    /// Gets or sets the provider's alias.
    /// </summary>
    string Alias { get; set; }

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

    /// <summary>
    /// Gets a collection of key/value pairs containing the options configured
    /// for a provider.
    /// </summary>
    /// <value>
    /// A collection of key/value pairs representing the options configured for
    /// the provider.
    /// </value>
    /// <remarks>The <see cref="Options"/> property represents the options
    /// configured for a provider by a user in the configuration repository.
    /// The options are defined by the provider itself and control the behavior
    /// within it. For example a provider may define options to support
    /// debugging/testing capabilities. Options are defined using a key-value
    /// syntax such as <c>debug=true</c>. The options should be defined using
    /// attributes of a Xml node that has the provider node as a parent and is
    /// named öptions". The provider stores the options as a
    /// <see cref="IDictionary&lt;TKey, TValue&gt;"/> so that the values may
    /// be retrieved using the key.
    /// <para>
    /// NOTE: There is no limit to the number of options a provider chooses to
    /// define.
    /// </para>
    /// <para>
    /// <example>
    /// <code>
    ///   ...
    ///   <provider name="SomeProvider">
    ///     <options
    ///      debug="true"
    ///      http-port="8080"
    ///      server-address="192.168.0.1">
    ///   </provider>
    ///   ..
    /// </code>
    /// The debug, http-port and server-address are the options configured for
    /// the provider "SomeProvider".
    /// </example>
    /// </para>
    /// </remarks>
    IDictionary<string, string> Options { get; }
  }
}
