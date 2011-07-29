using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using Nohros.Resources;

namespace Nohros.Configuration
{
    public class ChainNode : ConfigurationNode
    {
        string[] nodes_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ChainNode class by using the specified chain name.
        /// </summary>
        /// <param name="name"></param>
        public ChainNode(string name): base(name) { }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a chain.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <param name="config">The configuration object which this node belongs to.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a chain.</exception>
        public void Parse(XmlNode node) {
            List<string> nodes = new List<string>();
            foreach (XmlNode n in node.ChildNodes) {
                if (string.Compare(n.Name, "node", StringComparison.OrdinalIgnoreCase) == 0) {
                    string name = null;
                    if (!GetAttributeValue(n, "name", out name))
                        throw new ConfigurationErrorsException(string.Format(StringResources.Config_MissingAt, "name", NohrosConfiguration.kChainNodeTree + "." + name_));
                    nodes.Add(name);
                }
            }
            nodes_ = nodes.ToArray();
        }

        /// <summary>
        /// Gets the nodes that compose the chain.
        /// </summary>
        public string[] Nodes {
            get { return nodes_; }
        }
    }
}
