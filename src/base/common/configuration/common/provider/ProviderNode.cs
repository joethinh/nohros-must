using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// Serves as the base class for custom <see cref="IProviderNode"/>.
  /// </summary>
  public partial class ProviderNode : AbstractConfigurationNode, IProviderNode
  {
    /// <summary>
    /// Gets the provider's alias.
    /// </summary>
    protected string alias;

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
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the provider type.
    /// </param>
    protected ProviderNode(string name, string type)
      : this(name, string.Empty, type, AppDomain.CurrentDomain.BaseDirectory) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProviderNode"/> class by
    /// using the specified provider name, type and alias.
    /// </summary>
    /// <param name="name">
    /// The name of the provider.
    /// </param>
    /// <param name="type">
    /// The assembly-qualified name of the provider type.
    /// </param>
    /// <param name="alias">
    /// The provider's alias.
    /// </param>
    protected ProviderNode(string name, string alias, string type)
      : this(name, alias, type, AppDomain.CurrentDomain.BaseDirectory) {
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
    /// <param name="alias">
    /// The provider's alias.
    /// </param>
    protected ProviderNode(string name, string alias, string type,
      string location) : base(name) {
      if (type == null || location == null) {
        throw new ArgumentNullException(type == null ? "type" : "location");
      }
      this.type = type;
      this.location = location;
      this.alias = alias;
      options = new Dictionary<string, string>();
    }
    #endregion

    #region IProviderNode Members
    /// <inheritdoc/>
    public string Type {
      get { return type; }
    }

    /// <inheritdoc/>
    public virtual string Location {
      get { return location; }
    }

    /// <inheritdoc/>
    public virtual string Alias {
      get { return alias; }
      set { alias = value; }
    }

    /// <inheritdoc/>
    public IDictionary<string, string> Options {
      get { return options; }
    }
    #endregion
  }
}
