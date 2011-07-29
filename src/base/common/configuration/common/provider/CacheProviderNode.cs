using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Resources;
using Nohros.Data.Collections;

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
        /// <param name="base_path">The path to by used as base path for the location of the provider.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The
        /// <paramref name="node"/> is not a valid representation of a cache provider.</exception>
        /// <exception cref="ArgumentException">The <see cref="base_path"/> is not rooted.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="Node"/> or
        /// <paramref name="base_path"/> is a null reference.</exception>
        /// <remarks>
        /// <remarks>
        /// The location attribute of a provider could be absolute or relative. When it is relative we
        /// it will be resolved by using the specified <paramref name="base_path"/> path. An empty
        /// location will be resolved to the specified <paramref name="base_path"/> path.
        /// </remarks>
        public override void Parse(XmlNode node, string base_path) {

            if (node == null || base_path == null)
                throw new ArgumentNullException((node == null) ? "node" : "base_path");

            InternalParse(node, base_path);
        }
    }
}
