using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Resources;

namespace Nohros.Configuration
{
    /// <summary>
    /// Serves as the base class for custom <see cref="Nohros.Toolkit.IProviderNode"/>.
    /// </summary>
    public abstract class ProviderNode : ConfigurationNode, IProviderNode
    {
        internal const string kProvidersNodeName = "providers";

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
        /// Parses a XML node that contains information about a provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a provider.</exception>
        public override abstract void Parse(XmlNode node, NohrosConfiguration config);

        /// <summary>
        /// Gets the assembly-qualified name of the provider type.
        /// </summary>
        /// <seealso cref="AssemblyQualifiedName"/>
        public string Type {
            get { return type_; }
        }
    }
}
