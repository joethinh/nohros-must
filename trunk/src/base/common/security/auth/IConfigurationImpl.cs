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
        ListDictionary _modules;

        FileInfo _configFile;

        /// <summary>
        /// This class is used by the LoginConfiguration class to parse
        /// the login configuration file.
        /// </summary>
        public IConfigurationImpl()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string configFile = ConfigurationManager.AppSettings["LoginConfigurationFile"] as string;
            if (configFile == null)
                throw new LoginException("The LoginConfigurationFile key was not defined");

            if (configFile.StartsWith("~/"))
                configFile = Path.Combine(basePath, configFile.Substring(2));

            if (!File.Exists(configFile))
                throw new LoginException("The specified configuration file does not exists. Path:" + configFile);
            
            _configFile = new FileInfo(configFile);
            _modules = new ListDictionary();
        }

        /// <summary>
        /// Loads the configuration file and the login modules.
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref="LoadAndWatch(FileInfo, string)"/>
        public override void Load()
        {
            base.LoadAndWatch(_configFile, "//LoginModules");

            // load the login modules
            List<string> modules = new List<string>();
            foreach(XmlNode node in _element.ChildNodes)
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

                _modules[name] = entry;
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
            return _modules[name] as LoginModuleEntry;
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
                LoginModuleEntry[] entries = new LoginModuleEntry[_modules.Count];
                int i = 0;
                foreach(LoginModuleEntry entry in _modules.Values)
                    entries[i++] = entry;
                return entries;
            }
        }
    }
}