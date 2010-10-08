using System;
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

        string assembly_location_;

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
        }

        /// <summary>
        /// Parses a XML node that contains information about a provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrorsException">The <paramref name="node"/> is not a
        /// valid representation of a provider.</exception>
        public override abstract void Parse(XmlNode node, NohrosConfiguration config);

        /// <summary>
        /// Gets the assembly-qualified name of the provider type.
        /// </summary>
        /// <seealso cref="AssemblyQualifiedName"/>
        public string Type {
            get { return type_;}
        }

        /// <summary>
        /// Gets a string representing the fully qualified path to the directory where
        /// the assembly associated with the provider is located.
        /// </summary>
        /// <remarks>
        /// The AssemblyLocation must be an absolute path or a path relative to the configuration file.
        /// </remarks>
        public string AssemblyLocation {
            get { return assembly_location_; }
        }
    }
}
