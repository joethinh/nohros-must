using System;
using System.Collections.Generic;
using System.Xml;

namespace Nohros.Configuration
{
  /// <summary>
  /// Serves as the base class for custom <see cref="IProviderNode"/>.
  /// </summary>
  public partial class ProviderNode : AbstractConfigurationNode, IProviderNode
  {
    /// <summary>
    /// The path of the providers's assembly.
    /// </summary>
    protected string location;

    /// <summary>
    /// The options configured for this provider.
    /// </summary>
    /// <remarks>
    /// This should never be a <c>null</c> reference. If a provider does
    /// not have any configured options this dictionary should be empty.
    /// </remarks>
    protected IDictionary<string, string> options;

    /// <summary>
    /// The provider's assembly-qualified name.
    /// </summary>
    protected string type;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderNode"/> class by
    /// using the specified provider name and type.
    /// </summary>
    /// <param name="name">The name of the provider.</param>
    /// <param name="type">The assembly-qualified name of the provider
    /// type.</param>
    protected ProviderNode(string name, string type)
      : this(name, type, AppDomain.CurrentDomain.BaseDirectory) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderNode"/> class by
    /// using the specified provider name, type and assembly location.
    /// </summary>
    /// <param name="name">
    /// A string that uniquely identifies a provider within an collection.
    /// </param>
    /// <param name="type">
    /// The provider's assembly-qualified name.
    /// </param>
    /// <param name="location">
    /// The path to the directory where the provider assembly file is stored.
    /// </param>
    protected ProviderNode(string name, string type, string location)
      : base(name) {
      if (type == null || location == null) {
        throw new ArgumentNullException(type == null ? "type" : "location");
      }
      this.type = type;
      this.location = location;
      options = new Dictionary<string, string>();
    }
    #endregion

    #region IProviderNode Members
    /// <summary>
    /// Gets the assembly-qualified name of the provider type.
    /// </summary>
    /// <seealso cref="System.Type.AssemblyQualifiedName"/>
    public string Type {
      get { return type; }
    }

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
    /// </remarks>
    public string Location {
      get { return location; }
    }

    /// <inheritdoc/>
    public IDictionary<string, string> Options {
      get { return options; }
    }
    #endregion
  }
}
