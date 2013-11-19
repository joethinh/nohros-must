using System;
using System.Collections.Generic;

namespace Nohros.Configuration
{
  /// <summary>
  /// Serves as the base class for custom <see cref="IProviderNode"/>.
  /// </summary>
  public partial class ProviderNode : AbstractConfigurationNode, IProviderNode
  {
    readonly string type_;
    string[] aliases_;

    string group_;
    string location_;

    /// <summary>
    /// The options_ configured for this provider.
    /// </summary>
    /// <remarks>
    /// This should never be a <c>null</c> reference. If a provider does
    /// not have any configured options_ this dictionary should be empty.
    /// </remarks>
    IProviderOptions options_;

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
#if DEBUG
      if (type == null || location == null) {
        throw new ArgumentNullException(type == null ? "type" : "location");
      }
#endif
      type_ = type;
      location_ = location;
      group_ = string.Empty;
      options_ = new ProviderOptionsNode(name);
      aliases_ = new string[0];
    }
    #endregion

    /// <inheritdoc/>
    public string Type {
      get { return type_; }
    }

    /// <inheritdoc/>
    public virtual string Location {
      get { return location_; }
    }

    /// <inheritdoc/>
    public IProviderOptions Options {
      get { return options_; }
      protected set {
        if (options_ == null) {
          throw new ArgumentNullException("value");
        }
        options_ = value;
      }
    }

    /// <summary>
    /// Gets the group that this provider belongs to.
    /// </summary>
    public string Group {
      get { return group_; }
    }

    /// <inheritdoc/>
    public string[] Aliases {
      get { return aliases_; }
      protected set {
        if (aliases_ == null) {
          throw new ArgumentNullException("value");
        }
        aliases_ = value;
      }
    }

    /// <summary>
    /// Creates a deep copy of the current <see cref="ProviderNode"/>.
    /// </summary>
    /// <returns>
    /// A deep copy of the current <see cref="ProviderNode"/>.
    /// </returns>
    public ProviderNode Clone() {
      ProviderNode clone = new ProviderNode(name, type_, location_);
      foreach (KeyValuePair<string, string> option in options_) {
        clone.options_[option.Key] = option.Value;
      }

      List<string> list = new List<string>();
      foreach (string alias in aliases_) {
        list.Add(alias);
      }
      clone.aliases_ = list.ToArray();
      return clone;
    }
  }
}
