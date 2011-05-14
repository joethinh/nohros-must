using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Resources;

namespace Nohros.Configuration
{
    /// <summary>
    /// Serves as the base class for custom <see cref="IProviderNode"/>.
    /// </summary>
    public abstract class ProviderNode : ConfigurationNode, IProviderNode
    {
        const string kAssemblyLocationKey = "assembly-location";
        const string kNameAttribute = "name";
        const string kValueAttribute = "value";

        string assembly_location_;
        IDictionary<string, string> options_;

        /// <summary>
        /// The assembly-qualified name of the provider type.
        /// </summary>
        protected string type_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ProviderNode class by using the specified provider name
        /// and type.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="type">The assembly-qualified name of the provider type.</param>
        public ProviderNode(string name, string type): base(name) {
            if (type == null)
                throw new ArgumentNullException("type");
            type_ = type;
            options_ = null;
        }
        #endregion

        /// <summary>
        /// Parse data that are common to all providers.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <param name="config">The <see cref="NohrosConfiguration"/> object related with the provider.</param>
        /// <exception cref="ConfigurationErrorsException">The <paramref name="node"/> is not a valid
        /// representation of a data provider.</exception>
        internal void InternalParse(XmlNode node, NohrosConfiguration config) {
            string location = null;

            if (GetTrimmedAttributeValue(node, kAssemblyLocationKey, out location)) {
                // if the provider assembly location property is a relative path we need to resolve it
                // using the configuration file location. An empty string will be resolved to the
                // configuration file location.
                if (location != null && !Path.IsPathRooted(location))
                    location = Path.Combine(config.Location, location);
                assembly_location_ = location;
            }

            options_ = GetOptions(node);
        }

        /// <summary>
        /// Gets the options configured for a provider from the specified <see cref="XmlNode"/> provider node.
        /// </summary>
        /// <param name="node">A <see cref="XmlNode"/> node that represents a provider.</param>
        /// <returns>A <see cref="IDictionary&lt;TKey, TValue&gt;"/> containing the options configured for a provider.</returns>
        IDictionary<string, string> GetOptions(XmlNode node) {
            Dictionary<string, string> options = new Dictionary<string, string>();
            foreach (XmlNode n in node.ChildNodes) {
                string name, value;
                if (!(GetAttributeValue(n, kNameAttribute, out name) && GetAttributeValue(n, kValueAttribute, out value)))
                    throw new ConfigurationErrorsException(string.Format(StringResources.Provider_Attributes, "name", NohrosConfiguration.kProvidersNodeTree + "." + name_ + ".option"));
                options[name] = value;
            }
            return options;
        }

        /// <summary>
        /// Parses a XML node that contains information about a provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <param name="config">A <see cref="NohrosConfiguration"/> object containing the provider configuration
        /// informations.</param>
        /// <exception cref="ConfigurationErrorsException">The <paramref name="node"/> is not a
        /// valid representation of a provider.</exception>
        public override abstract void Parse(XmlNode node, NohrosConfiguration config);

        /// <summary>
        /// Gets the assembly-qualified name of the provider type.
        /// </summary>
        /// <seealso cref="System.Type.AssemblyQualifiedName"/>
        public string Type {
            get { return type_; }
            internal set { type_ = value; }
        }

        /// <summary>
        /// Gets a string representing the fully qualified path to the directory where
        /// the assembly associated with the provider is located.
        /// </summary>
        /// <value>
        /// The fully qualified path to the folder where the provider assembly is stored.
        /// </value>
        /// <remarks>
        /// The assembly location must be an absolute path or a path relative to the configuration file.
        /// </remarks>
        public string AssemblyLocation {
            get { return assembly_location_; }
            set { assembly_location_ = value; }
        }

        /// <summary>
        /// Gets a collection of key/value pairs containing the options configured for a provider.
        /// </summary>
        /// <value>
        /// A collection of key/value pairs representing the options configured for the provider.
        /// </value>
        /// <remarks>The <see cref="Options"/> property represents the options configured for a provider by a user
        /// in the configuration repository. The options are defined by the provider itself and control the
        /// behavior within it. For example a provider may define options to support debugging/testinz capabilities.
        /// Options are defined using a key-value syntaxm such as <c>debug=true</c>. The provider stores the options
        /// as a <see cref="IDictionary&lt;TKey, TValue&gt;"/> so that the values may be retrieved using the key.
        /// <para>
        /// NOTE: There is no limit to the number of options a provider chooses to define.
        /// </para>
        /// </remarks>
        public IDictionary<string, string> Options {
            get { return options_; }
            set { options_ = value; }
        }
    }
}