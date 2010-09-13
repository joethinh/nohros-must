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
            base.LoadAndWatch(config_file_, "//LoginModules");

            // load the login modules
            List<string> modules = new List<string>();
            foreach(XmlNode node in element_.ChildNodes)
            {
                string name = node.Name;
                XmlAttributeCollection atts = node.Attributes;

                XmlAttribute type = atts["type"] as XmlAttribute;
                if (type == null)
                    throw new LoginException(StringResources.Auth_Config_Missing_LoginModuleType);

                Type t = Type.GetType(type.Value);
                if (t == null)
                    throw new LoginException(StringResources.Auth_Config_InvalidModuleType);

                XmlAttribute flag = atts["flag"] as XmlAttribute;
                if (flag == null)
                    throw new LoginException(StringResources.Auth_Config_Missing_ControlFlag);

                // parse the control flag
                LoginModuleEntry.LoginModuleControlFlag controlFlag;
                try { controlFlag =
                        (LoginModuleEntry.LoginModuleControlFlag)
                            Enum.Parse(typeof(LoginModuleEntry.LoginModuleControlFlag), flag.Value.ToUpper()); }
                            catch { throw new LoginException(StringResources.Auth_Config_InvalidControlFlag); }

                // Gets the login module specific options
                Dictionary<string, object> options = GetOptions(node);

                LoginModuleEntry entry = new LoginModuleEntry(name, t, controlFlag, options);

                modules_[name] = entry;
            }
        }

        private Dictionary<string, object> GetOptions(XmlNode parent)
        {
            if (!parent.HasChildNodes)
                return null;

            Dictionary<string, object> options = new Dictionary<string, object>();

            foreach (XmlNode node in parent)
            {
                string name = node.Name;
                string value = node.InnerText;
                string val = null;

                if (value.StartsWith("${"))
                {
                    try
                    {
                        val = value;
                        value = System.Environment.ExpandEnvironmentVariables(value.Substring(3, value.Length - 3));
                    }
                    catch { value = val; }
                }
                options.Add(name, value);
            }
            return options;
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