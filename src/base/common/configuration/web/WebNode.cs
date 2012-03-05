using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

using Nohros.Data;
using Nohros.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class WebNode : ConfigurationNode
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the WebNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="node">A XML node that contains the configuration data.</param>
        /// <param name="common_node">The configuration common node.</param>
        public WebNode() : base(NohrosConfiguration.kWebNodeName) { }
        #endregion

        string ContentGroupKey(string name, string build, string mime_type) {
            return string.Concat(name, ".", build, ".", mime_type);
        }

        /// <summary>
        /// Parses a XML node that contains information about a web node.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <param name="nodes">A <see cref="DictionaryValue"/> containing the collection of
        /// configuration nodes.</param>
        /// <remarks>
        /// The <paramref name="nodes"/> is used to store the nodes that is parsed by this class.
        /// </remarks>
        public void Parse(XmlNode node, DictionaryValue nodes) {
            XmlNode xml_web_node;
            DictionaryValue<ContentGroupNode> content_groups;

            if (GetNode<ContentGroupNode>(NohrosConfiguration.kContentGroupsNodeName
                    ,NohrosConfiguration.kContentGroupNodeTree
                    ,nodes
                    ,node
                    ,out xml_web_node
                    ,out content_groups)) {

                DictionaryValue<RepositoryNode> repositories =
                    (DictionaryValue <RepositoryNode>)nodes[NohrosConfiguration.kRepositoryNodeTree];

                foreach (XmlNode n in xml_web_node.ChildNodes) {
                    if (string.Compare(n.Name, "group", StringComparison.OrdinalIgnoreCase) == 0) {
                        string name;
                        if (!GetAttributeValue(n, "name", out name))
                            Thrower.ThrowConfigurationException("[Parse   Nohros.Configuration.WebNode]",
                                string.Format(StringResources.Config_MissingAt, "name",
                                NohrosConfiguration.kContentGroupNodeTree));

                        ContentGroupNode content_group = new ContentGroupNode(name);
                        content_group.Parse(n, repositories);
                        content_groups[ContentGroupKey(content_group.Name,
                            (content_group.BuildType == BuildType.Release) ?
                                "release" : "build", content_group.MimeType)] = content_group;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of files related with the specified content group, build version and mime type.
        /// </summary>
        /// <param name="name">The name of the group to get.</param>
        /// <param name="build">The build version</param>
        /// <param name="mime_type">The mime type of the group.</param>
        /// <returns></returns>
        public ContentGroupNode GetContentGroup(string name, string build, string mime_type) {
            if (name == null || build == null || mime_type == null)
                throw new ArgumentException((name == null) ? (build == null) ? "mime_type" : "build" : "name");
            return this[ContentGroupKey(name, build, mime_type)] as ContentGroupNode;
        }
    }
}
