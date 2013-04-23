using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// Defines the methods and properties that all the configuration nodes that
  /// are related with some type of provider should implements.
  /// </summary>
  public interface IProviderNode : IConfigurationNode, IRuntimeType
  {
    /// <summary>
    /// Gets a string that identifies the group that a provider belongs to.
    /// </summary>
    /// <remarks>
    /// If a provider is not associated with any group, this property returns
    /// a empty string.
    /// </remarks>
    string Group { get; }

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
    IProviderOptions Options { get; }

    /// <summary>
    /// Gets a collection of strings that can be used to identify the provider.
    /// </summary>
    /// <remarks>
    /// <see cref="Aliases"/> provides a way to identify the same provider by
    /// more than one name. This property is useful when you need to
    /// implement distinct provider behavior within a single class and do not
    /// want to couple the distinct behaviors within a single definition.
    /// <example>
    /// <code>
    /// public interface ISomeRepository {
    ///   string SomeProperty { get; }
    /// }
    /// <para>
    /// <code>
    /// public interface ISomeOtherRepository {
    ///   string SomeOtherProperty { get; }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// <code>
    /// public SqlDataProvider : ISomeRepository, ISomeOtherRepository {
    /// }
    /// </code>
    /// </para>
    /// </code>
    /// </example>
    /// The SqlDataProvider could be referenced in the configuration file
    /// using a single provider node and two alias.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// An attempt to set the value of the property to null was performed.
    /// </exception>
    ICollection<string> Aliases { get; }
  }
}
