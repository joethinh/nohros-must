using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nohros.Configuration
{
    /// <summary>
    /// Represents a single node in the nohros configuration file.
    /// </summary>
    internal interface IConfigurationNode
    {
        /// <summary>
        /// Parses the content of the XML node.
        /// </summary>
        void Parse(XmlNode node);

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        string Name { get; }
    }
}