using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Nohros.Net
{
    internal class NetSettings
    {
        string style_path_;
        string script_path_;
		bool is_path_in_url_;
        string plugins_path_;

        // store the plugins and scripts information that will replace the defaults
        List<string> plugins_;
        ListDictionary scripts_;

        #region .ctor
        public NetSettings()
		{
			style_path_ = Utility.MapPath("~/css");
			script_path_ = Utility.MapPath("~/js");
			is_path_in_url_ = false;
            scripts_ = new ListDictionary();
            plugins_ = new List<string>();
            plugins_path_ = null;
        }
        #endregion

        /// <summary>
        /// Gets or sets the path where the CSS files are stored.
        /// </summary>
        public string StylePath
        {
            get { return style_path_; }
			set { style_path_ = Utility.MapPath(value); }
        }

        /// <summary>
        /// Gets or sets the path where the javascript(.js) files are stored.
        /// </summary>
        public string ScriptPath
        {
            get { return script_path_; }
			set { script_path_ = Utility.MapPath(value); }
        }

        /// <summary>
        /// Gets a value indication wheter the path of the merge file is within the
        /// requested URL or not.
        /// </summary>
		public bool IsPathInUrl
		{
			get { return is_path_in_url_; }
			set { is_path_in_url_ = value; }
		}

        /// <summary>
        /// Gets a list of user defined plugins that will be added to the output
        /// </summary>
        public List<string> Plugins
        {
            get { return plugins_; }
        }

        /// <summary>
        /// Gets a table containing the scripts that will replace the library-defined scripts.
        /// </summary>
        public ListDictionary Scripts
        {
            get { return scripts_; }
        }

        /// <summary>
        /// Gets or sets the path where the plugins files are stored.
        /// </summary>
        public string PluginsPath {
            get { return plugins_path_; }
            set { plugins_path_ = (value.EndsWith("\\")) ? value : value + "\\"; }
        }
    }
}