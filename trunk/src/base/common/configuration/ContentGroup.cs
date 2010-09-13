using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;

namespace Nohros.Configuration
{
    public class ContentGroup : Value
    {
        string base_path_;
        string name_;
        BuildType build_type_;
        string mime_type_;
        List<string> files_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ContentGroup by using the specified group name, build and mime.
        /// </summary>
        /// <param name="name">The name of the content group.</param>
        /// <param name="build">The content group build type(release, debug, ...)</param>
        /// <param name="mime_type">The related MIME type.</param>
        public ContentGroup(string name, BuildType build_type, string mime_type):base(ValueType.TYPE_GENERIC) {
            name_ = name;
            build_type_ = build_type;
            mime_type_ = mime_type;
            files_ = new List<string>();
            base_path_ = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Initializes a new instance of the ContentGroup class by using the specified group name, build and mime type,
        /// base path and files.
        /// </summary>
        /// <param name="name">The name of the content group.</param>
        /// <param name="build">The content group build type(release, debug, ...)</param>
        /// <param name="mime_type">The related MIME type.</param>
        /// <param name="base_path">The base path where the files are stored on disk.</param>
        /// <param name="files">The files that compose the content group.</param>
        public ContentGroup(string name, BuildType build_type, string mime_type, string base_path, List<string> files): base(ValueType.TYPE_GENERIC) {
            name_ = name;
            build_type_ = build_type;
            mime_type_ = mime_type;
            base_path_ = base_path;
            files_ = files;
        }
        #endregion

        /// <summary>
        /// Gets or sets a fully qualified path to the folder where the files that compose the content group are stored.
        /// </summary>
        public string BasePath {
            get { return base_path_; }
            set { base_path_ = value; }
        }

        /// <summary>
        /// Gets the name of the content group.
        /// </summary>
        public string Name {
            get { return name_; }
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
