using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Resources;
using Nohros.Collections;

namespace Nohros.Configuration
{
  /// <summary>
  /// Serves as the base class for custom <see cref="IProviderNode"/>.
  /// </summary>
  public abstract class ProviderNode: AbstractConfigurationNode, IProviderNode
  {
    const string kAssemblyLocationKey = "assembly-location";
    const string kNameAttribute = "name";
    const string kValueAttribute = "value";
    const string kOptionsNodeName = "options";

    /// <summary>
    /// The path of the providers's assembly.
    /// </summary>
    protected string assembly_location;

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
    /// Initializes a new instance of the ProviderNode class by using the
    /// specified provider name and type.
    /// </summary>
    /// <param name="name">The name of the provider.</param>
    /// <param name="type">The assembly-qualified name of the provider
    /// type.</param>
    protected ProviderNode(string name, string type): base(name) {
      if (type == null) {
        throw new ArgumentNullException("type");
      }
      this.type = type;
      options = new Dictionary<string, string>();
      assembly_location = AppDomain.CurrentDomain.BaseDirectory;
      options = new Dictionary<string, string>();
    }
    #endregion

    /// <summary>
    /// Parse data that are common to all providers.
    /// </summary>
    /// <param name="node">The XML node to parse.</param>
    /// <param name="base_path">The path to by used as base path for the
    /// location of the provider.
    /// </param>
    /// <exception cref="ConfigurationErrorsException">The
    /// <paramref name="node"/> is not a valid representation of a data
    /// provider.</exception>
    /// <exception cref="ArgumentException">The <see cref="base_path"/> is not
    /// rooted.</exception>
    /// <remarks>
    /// The location attribute of a provider could be absolute or relative.
    /// When it is relative we it will be resolved by using the specified
    /// <paramref name="base_path"/> path. An empty location will be resolved
    /// to the specified <paramref name="base_path"/> path.
    /// <para>
    /// If the location attribute is not specified the provider assembly
    /// location property will be set to the specified
    /// <paramref name="base_path"/>.
    /// </para>
    /// </remarks>
    internal void InternalParse(XmlNode node, string base_path) {
      if (!Path.IsPathRooted(base_path))
        throw new ArgumentException(
          string.Format(StringResources.Config_path_is_not_rooted, base_path));

      string location = null;

      GetTrimmedAttributeValue(node, kAssemblyLocationKey, out location);

      // if the provider assembly location property is a relative path we
      // need to resolve it using the configuration file location. An empty
      // string will be resolved to the configuration file location.
      if (location != null && !Path.IsPathRooted(location)) {
        location = Path.Combine(base_path, location);
      } else if (location == null || location.Trim().Length == 0) {
        location = base_path;
      }
      assembly_location = location;

      foreach (XmlNode n in node.ChildNodes) {
        if (string.Compare(n.Name, kOptionsNodeName,
          StringComparison.OrdinalIgnoreCase) == 0) {

          // get the options from the attributes of
          // the first child node whose name is "options".
          options = GetOptions(n);
          break;
        }
      }

    }

    /// <summary>
    /// Gets the options configured for a provider from the specified
    /// <see cref="XmlNode"/> provider node.
    /// </summary>
    /// <param name="node">A <see cref="XmlNode"/> node that represents a
    /// provider.</param>
    /// <returns>A <see cref="IDictionary&lt;TKey, TValue&gt;"/> containing the
    /// options configured for a provider.</returns>
    IDictionary<string, string> GetOptions(XmlNode node) {
      Dictionary<string, string> options = new Dictionary<string, string>();
      foreach(XmlAttribute attribute in node.Attributes) {
        options[attribute.Name] = attribute.Value;
      }
      return options;
    }

    /// <summary>
    /// Parses a XML node that contains information about a provider.
    /// </summary>
    /// <param name="node">The XML node to parse.</param>
    /// <param name="nodes">A dictionary where the parsed provider could be
    /// stored.</param>
    /// <param name="base_path">A string representing the base path of the
    /// provider physical location.</param>
    /// <exception cref="ConfigurationErrorsException">The
    /// <paramref name="node"/> is not a valid representation of a
    /// provider.</exception>
    public abstract void Parse(XmlNode node, string base_path);

    /// <summary>
    /// Gets the assembly-qualified name of the provider type.
    /// </summary>
    /// <seealso cref="System.Type.AssemblyQualifiedName"/>
    public string Type {
      get { return type; }
      protected set { type = value; }
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
    /// <para>
    /// If the <see cref="Parse(XmlNode, base_path)"/> was not called attempt
    /// to get the value of this property will return the path of the
    /// application base directory.
    /// </para>
    /// </remarks>
    public string AssemblyLocation {
      get { return assembly_location; }
      set { assembly_location = value; }
    }

    /// <inheritdoc/>
    public IDictionary<string, string> Options {
      get { return options; }
      set { options = value; }
    }
  }
}