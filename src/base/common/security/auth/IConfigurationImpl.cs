using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;

using Nohros.Configuration;
using Nohros.Resources;

namespace Nohros.Security.Auth
{
    internal class IConfigurationImpl : IConfiguration
    {
        ListDictionary modules_;
        FileInfo config_file_;

        /// <summary>
        /// This class is used by the LoginConfiguration class to parse the login configuration file.
        /// </summary>
        public IConfigurationImpl()
        {
            string base_path = AppDomain.CurrentDomain.BaseDirectory;
            string config_file = ConfigurationManager.AppSettings["LoginConfigurationFile"] as string;
            if (config_file == null)
                throw new LoginException("The LoginConfigurationFile key was not defined");

            if (config_file.StartsWith("~/"))
                config_file = Path.Combine(base_path, config_file.Substring(2));

            if (!File.Exists(config_file))
                throw new LoginException("The specified configuration file does not exists. Path:" + config_file);
            
            config_file_ = new FileInfo(config_file);
            modules_ = new ListDictionary();
        }

        /// <summary>
        /// Loads the configuration file and the login modules.
        /// </summary>
        /// <remarks></remarks>
        public override void Load()
        {
        }

        /// <summary>
        /// Retrieve the LoginModuleEntry for the specified name
        /// </summary>
        /// <param name="name">the name used to index the module</param>
        /// <returns>A LoginModuleEntry for the spcified <paramref name="name"/>,
        /// or null if there are no entry for the specified <paramref name="name"/></returns>
        public LoginModuleEntry GetLoginModuleEntry(string name)
        {
            return modules_[name] as LoginModuleEntry;
        }

        /// <summary>
        /// Gets all the login modules configured for the application.
        /// </summary>
        /// <returns>An array of LoginModuleEntry containg all the login
        /// modules configured for the application</returns>
        public LoginModuleEntry[] ModuleEntries
        {
            get
            {
                LoginModuleEntry[] entries = new LoginModuleEntry[modules_.Count];
                int i = 0;
                foreach(LoginModuleEntry entry in modules_.Values)
                    entries[i++] = entry;
                return entries;
            }
        }
    }
}