using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Resources;

namespace Nohros.Configuration
{
    public class WebNode : ConfigurationNode
    {
        const string kWebNodeName = "web";
        internal const string kNodeTree = kWebNodeName + ".";

        const string kContentGroupsNodeName = "content-groups";

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the CommonNode class by using the specified XML node and name.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="node">A XML node that contains the configuration data.</param>
        /// <param name="common_node">The configuration common node.</param>
        public WebNode(CommonNode common_node)
            : base((common_node == null) ? kWebNodeName : string.Concat(common_node.Name, ".", kWebNodeName), common_node) { }
        #endregion

        /// <summary>
        /// Instantiate a new instane of the <see cref="WebNode"/> class and parses the specified node.
        /// </summary>
        /// <param name="node">A XML node that contains the configuration data.</param>
        /// <returns></returns>
        public static WebNode FromXmlNode(XmlNode node, CommonNode common_node) {
            WebNode web_node = new WebNode(common_node);
            web_node.Parse(node);
            return web_node;
        }

        string ContentGroupKey(string name, string build, string mime_type) {
            return string.Concat(kContentGroupsNodeName + "." + name, ".", build, ".", mime_type);
        }

        /// <summary>
        /// Parses the XML node.
        /// </summary>
        public override void Parse(XmlNode node) {
            XmlNode data_node = SelectNode(node, kContentGroupsNodeName);
            if (data_node == null)
                return; // content-group nodes are not mandatory

            foreach (XmlNode n in data_node.ChildNodes) {
                if (string.Compare(n.Name, "group", StringComparison.OrdinalIgnoreCase) == 0) {
                    string name;
                    if (!GetAttributeValue(n, kContentGroupsNodeName, out name))
                        // TODO: Log the exception
                        Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "name", kNodeTree + kContentGroupsNodeName));

                    ContentGroupNode content_group = new ContentGroupNode(name, this);
                    content_group.Parse(n);
                    this[ContentGroupKey(content_group.Name, (content_group.BuildType == BuildType.Release) ? "release" : "build", content_group.Name)] = content_group;
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
