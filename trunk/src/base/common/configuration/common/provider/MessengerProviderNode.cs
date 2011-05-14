using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
    /// <summary>
    /// Contains configuration informations for messenger providers.
    /// </summary>
    public class MessengerProviderNode : ProviderNode
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="MessengerProviderNode"/>MessengerProviderNode class by using
        /// the provider name and data type.
        /// </summary>
        /// <param name="name">The name of the messenger.</param>
        /// <param name="type">The assembly-qualified type of the provider.</param>
        public MessengerProviderNode(string name, string type) : base(name, type) { }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a provider.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <param name="config">A NohrosConfiguration object containing the provider configuration
        /// informations.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The <paramref name="node"/> is not a
        /// valid representation of a messenger provider.</exception>
        public override void Parse(XmlNode node, NohrosConfiguration config) {
            InternalParse(node, config);
        }
    }
}