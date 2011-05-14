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
    /// Contains configuration informations for cache providers.
    /// </summary>
    public class CacheProviderNode : ProviderNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProviderNode"/> class by using the specified
        /// provider name and type.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="type">The assembly-qualified name of the provider type.</param>
        public CacheProviderNode(string name, string type) : base(name, type) { }

        /// <summary>
        /// Parses a XML node that contains information about the provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <param name="config">A <see cref="NohrosConfiguration"/> object containing the provider configuration
        /// informations.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The <paramref name="node"/> is not a
        /// valid representation of a messenger provider.</exception>
        public override void Parse(XmlNode node, NohrosConfiguration config) {
            InternalParse(node, config);
        }
    }
}
