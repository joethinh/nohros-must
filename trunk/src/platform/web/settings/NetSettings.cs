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

namespace Nohros.Net
{
    internal class NetSettings
    {
        DictionaryValue config_;

        #region Constants
        public const string kPathNs = "paths";
        public const string kCssPath = "css";
        public const string kJsPath = "js";
        public const string kPluginsPath = "plugins";
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the NetSettings class.
        /// </summary>
        /// <exception cref="KeyNotFoundException">the registry_key "NohrosConfigurationFile" was not found into the application
        /// configuration file.</exception>
        /// <exception cref="FileNotFoundException">The file pointed by the "NohrosConfigurationFile" registry_key value does not
        /// exists.</exception>
        public NetSettings()
        {
            string config_file_path = ConfigurationManager.AppSettings["NohrosConfigurationFile"];
            if(config_file_path == null || config_file_path.Length == 0)
                throw new KeyNotFoundException(string.Format(StringResources.Config_KeyNotFound, "NohrosConfigurationFile"));

            Configure(config_file_path);
        }

        /// <summary>
        /// Initializes a new instance of the NetSettings class by using the specifed configuration file path.
        /// </summary>
        /// <param name="config_file_path">The path to the configuration file</param>
        /// <exception cref="FileNotFoundException"><paramref name="config_file_path"/> was not found</exception>
        public NetSettings(string config_file_path)
		{
            Configure(config_file_path);
        }
        #endregion

        /// <summary>
        /// Reads and parses the configuration file.
        /// </summary>
        /// <param name="config_file_path">The fully qualified path of the configuration file</param>
        void Configure(string config_file_path) {
            if( config_file_path.StartsWith("~/"))
                config_file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config_file_path.Substring(2));

            if (!File.Exists(config_file_path))
                throw new FileNotFoundException(string.Format(StringResources.Config_FileNotFound_Path));

            JSONReader reader = new JSONReader();
            using (StreamReader file_reader = new StreamReader(config_file_path)) {
                string json = file_reader.ReadToEnd();
                config_ = reader.JsonToValue(json, true, true) as DictionaryValue;
            }
        }

        /// <summary>
        /// Gets a Value object associated with the specified <paramref name="registry_key"/>.
        /// </summary>
        /// <param name="registry_key"></param>
        /// <returns></returns>
        public Value this[string key] {
            get {
                return config_.Get(key);
            }
        }
    }
}