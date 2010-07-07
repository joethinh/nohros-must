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
        public const string kPathNs = "paths";
        public const string kCssPath = "css";
        public const string kJsPath = "js";
        public const string kPluginsPath = "plugins";

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the NetSettings class.
        /// </summary>
        /// <remarks>
        /// If the name of the configuration file is specified into the main application file, that file must
        /// have a node with the following xpath "//nohros/net" containing the configuration data.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">the key "NohrosConfigurationFile" was not found into the application
        /// configuration file.</exception>
        /// <exception cref="FileNotFoundException">The file pointed by the "NohrosConfigurationFile" key value does not
        /// exists.</exception>
        public NetSettings()
        {
            string config_file_path = ConfigurationManager.AppSettings["NohrosConfigurationFile"];
            if(config_file_path == null || config_file_path.Length == 0)
                throw new KeyNotFoundException(string.Format(StringResources.Config_KeyNotFound, "NohrosConfigurationFile"));

            //(config_file_path, "//nohros/net");
        }

        /// <summary>
        /// Initializes a new instance of the NetSettings class by using the specifed configuration file path.
        /// </summary>
        /// <param name="config_file_path">The path to the configuration file</param>
        /// <param name="root_node_name"></param>
        /// <exception cref="FileNotFoundException"><paramref name="config_file_path"/> was not found</exception>
        public NetSettings(string config_file_path, string root_node_name)
		{
            if (config_file_path.StartsWith("~/"))
                config_file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config_file_path.Substring(2));

            Load(config_file_path, root_node_name);
        }
        #endregion

        /// <summary>
        /// Gets a Value object associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value this[string key] {
            get {
                return config_.Get(key);
            }
        }
    }
}