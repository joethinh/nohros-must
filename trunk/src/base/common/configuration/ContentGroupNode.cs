using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Resources;

namespace Nohros.Configuration
{
    public class ContentGroupNode : ConfigurationNode
    {
        const string kNodeTree = WebNode.kNodeTree + "content-groups.";

        const string kFileNameAttributeName = "file-name";
        const string kNameAttributeName = "name";
        const string kBuildAttributeName = "build";
        const string kMimeTypeAttributeName = "mime-type";
        const string kPathRefAttributeName = "path-ref";
        const string kDataBaseOwnerAttributeName = "dbowner";
        const string kConnectionStringAttributeName = "dbstring";

        string base_path_;
        BuildType build_type_;
        string mime_type_;
        List<string> files_;

        WebNode web_node_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the ContentGroupNode by using the specified name and parent
        /// node.
        /// </summary>
        /// <param name="common_node">A WebNode object which this provider belongs.</param>
        public ContentGroupNode(string name, WebNode web_node): base(name, web_node) {
            web_node_ = web_node;
            files_ = new List<string>();
            base_path_ = AppDomain.CurrentDomain.BaseDirectory;
            mime_type_ = null;
            build_type_ = BuildType.Release;
        }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a content group.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a content group.</exception>
        public override void Parse(XmlNode node) {
            string name = null, build = null, mime_type = null, path_ref = null;
            if (!(GetAttributeValue(node, kNameAttributeName, out name) &&
                    GetAttributeValue(node, kBuildAttributeName, out build) &&
                    GetAttributeValue(node, kMimeTypeAttributeName, out mime_type) &&
                    GetAttributeValue(node, kPathRefAttributeName, out path_ref)
                )) {
                // TODO: log the exception.
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, "a required attribute", kNodeTree + ".any"));
            }

            // sanity check the build type
            if (build != "release" && build != "debug")
                // TODO: log the exception.
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ArgOutOfRange, build, kNodeTree + name + ".build"));

            // resolve the base path
            string str = path_ref;
            CommonNode common_node = web_node_.ParentNode as CommonNode;
            if (!common_node.GetRepository(str, out path_ref))
                Thrower.ThrowConfigurationException(string.Format(StringResources.Config_ArgOutOfRange, path_ref, kNodeTree + name + ".path-ref"));

            build_type_ = (build == "release") ? BuildType.Release : BuildType.Debug;
            mime_type_ = mime_type;
            base_path_ = path_ref;

            string file_name = null;
            foreach (XmlNode file_node in node.ChildNodes) {
                if (string.Compare(file_node.Name, "add", StringComparison.OrdinalIgnoreCase) == 0) {
                    if (!GetAttributeValue(file_node, kFileNameAttributeName, out file_name)) {
                        // TODO: log the exception.
                        Thrower.ThrowConfigurationException(string.Format(StringResources.Config_MissingAt, kFileNameAttributeName, Name));
                    }
                    files_.Add(file_name);
                }
            }
        }

        /// <summary>
        /// Gets or sets a fully qualified path to the folder where the files that compose the content group are stored.
        /// </summary>
        public string BasePath {
            get { return base_path_; }
            set { base_path_ = value; }
        }

        /// <summary>
        /// Gets the content group build version.
        /// </summary>
        /// <remarks>
        /// The build version could be used to discriminate diferrent types of files for diferrent types
        /// of deployment cenarios. Usually a release version of a JScript file is mimimized to speed the
        /// load time, elsewhere, a debug version of a JSScript file may be huge and may contains a lot of
        /// comments.
        /// </remarks>
        public BuildType BuildType {
            get { return build_type_; }
        }

        /// <summary>
        /// Gets the related MIME type.
        /// </summary>
        public string MimeType {
            get { return mime_type_; }
        }

        /// <summary>
        /// Gets the files that composes the content group
        /// </summary>
        public List<string> Files {
            get { return files_; }
            set { files_ = value; }
        }
    }
}
