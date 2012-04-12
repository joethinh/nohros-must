using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

using Nohros.Collections;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class RepositoryNode : AbstractConfigurationNode
    {
        string path_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the RepositoryNode class by using the specified repository name.
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        /// <remarks>
        /// The path of the repository will be set to the application base directory.
        /// </remarks>
        public RepositoryNode(string name): base(name) {
            path_ = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the RepositoryNode by using the specified repository name and path.
        /// </summary>
        /// <param name="name">A string that identifies the repository.</param>
        /// <param name="path">The application relative path of the repository.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="path"/> is
        /// a null reference.</exception>
        /// <remarks>The existence of the physical path is not checked.
        /// </remarks>
        public RepositoryNode(string name, string path):base(name) {
            if (path == null)
                throw new ArgumentNullException("path");
            path_ = path;
        }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a repository.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a data provider or the repository node contains an rooted path.</exception>
        public void Parse(XmlNode node) {
            string relative_path = null;
            if (!GetAttributeValue(node, "relative-path", out relative_path))
                throw new ConfigurationErrorsException(string.Format(StringResources.Config_ErrorAt, "attribute name", NohrosConfiguration.kRepositoryNodeTree + "." + name));

            Path = relative_path;
        }

        /// <summary>
        /// Gets or sets the path pointed by the repository.
        /// </summary>
        /// <remarks>
        /// Attempt to get the value of this property return the fully qualified path of the repository.
        /// While setting the value of this property the caller must specifies a non-rooted path that is
        /// relative to the application base directory.
        /// </remarks>
        public string Path {
            get { return path_; }
            set {
                // sanity check if the path is rooted. It must be relative to the application base
                // directory.
                if (System.IO.Path.IsPathRooted(value))
                    throw new ConfigurationErrorsException(string.Format(StringResources.Config_PathIsRooted, value));

                path_ = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value);
            }
        }
    }
}
