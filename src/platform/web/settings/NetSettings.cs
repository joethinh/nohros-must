using System;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

using Nohros;
using Nohros.Data;
using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Net
{
    internal class NetSettings : IConfiguration
    {
        const string kPathsKey = "paths.";
        const string kCssPathKey = "paths.css";
        const string kJsPathKey = "paths.js";
        const string kPluginsPathKey = "paths.plugins";
        const string kMergeGroupsKey = "groups.";

        string config_file_path_, root_node_name_, js_path_, css_path_, plugins_path_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the NetSettings class by using the specifed configuration file path.
        /// </summary>
        /// <param name="config_file_path">The path to the configuration file</param>
        /// <param name="root_node_name"></param>
        /// <exception cref="FileNotFoundException"><paramref name="config_file_path"/> was not found</exception>
        public NetSettings(string config_file_path, string root_node_name):base(null, "settings")
		{
            if (config_file_path.StartsWith("~/"))
                config_file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config_file_path.Substring(2));

            config_file_path_ = config_file_path;
            root_node_name_ = root_node_name;
        }
        #endregion

        public enum ContentType {
            HTML,
            Javascript,
            StyleSheet
        }

        /// <summary>
        /// Loads and parses the configuration file.
        /// </summary>
        public override void Load() {
            Load(config_file_path_, root_node_name_);
            
            // parse the paths
            string string_value, base_dir = AppDomain.CurrentDomain.BaseDirectory;

            string_value = this[kCssPathKey];
            css_path_ = (string_value == null) ? Path.Combine(base_dir, "css") : string_value;

            string_value = this[kPluginsPathKey];
            plugins_path_ = (string_value == null) ? Path.Combine(base_dir, "plugins") : string_value;

            string_value = this[kJsPathKey];
            js_path_ = (string_value == null) ? Path.Combine(base_dir, "js") : string_value;
        }

        /// <summary>
        /// Gets a path related with the specified path.
        /// </summary>
        /// <param name="name">A string that identifies the path to retrieve.</param>
        /// <returns>A string that contains the path related with the specified name.</returns>
        public string GetPath(string name) {
            return this[kPathsKey + name];
        }

        public string[] GetStyleSheetFiles(string group_name) {
            return GetGroup(kCssPathKey + name);
        }

        public string[] GetJavascriptFiles(string group_name) {
            return GetGroup(kCssPathKey + name);
        }

        string[] GetGroupFiles(string path, string group_name) {
            DictionaryValue value = Get(key) as DictionaryValue;
            if (value != null) {

            }
        }

        /// <summary>
        /// Gets the path where the java script files are stored.
        /// </summary>
        public string JsPath {
            get { return js_path_; }
        }

        /// <summary>
        /// Gets the path where the stylesheet files are stored.
        /// </summary>
        public string CssPath {
            get { return css_path_; }
        }

        /// <summary>
        /// Gets the path where the plugins files are stored.
        /// </summary>
        public string PluginsPath {
            get { return plugins_path_; }
        }
    }
}