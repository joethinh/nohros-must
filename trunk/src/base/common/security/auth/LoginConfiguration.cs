using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Data;
using Nohros.Configuration;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A LoginConfigur
    /// </summary>
    public class LoginConfiguration : NohrosConfiguration, ILoginConfiguration
    {
        static LoginConfiguration instance_;

        LoginModuleNode[] modules_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the LoginConfiguration class.
        /// </summary>
        public LoginConfiguration()
        {
            modules_ = null;
        }

        /// <summary>
        /// Singleton initializer.
        /// </summary>
        static LoginConfiguration()
        {
            instance_ = new LoginConfiguration();
            instance_.Load();

            // we need to store a reference to all nodes configured for a specific application.
            // this will be stored into a array for faster retrieval.
            if (instance_.CommonNode != null) {
                List<ConfigurationNode> nodes = instance_.CommonNode.ChildNodes;
                if (nodes != null && nodes.Count > 0) {
                    List<LoginModuleNode> modules = new List<LoginModuleNode>(nodes.Count);
                    foreach (LoginModuleNode module in modules) {
                        modules.Add(module);
                    }
                    instance_.modules_ = modules.ToArray();
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the single instance_ of the ILoginConfiguration class.
        /// </summary>
        /// <returns>A instance_ of the ILoginConfiguration class</returns>
        /// <remarks>
        /// The default LoginConfiguration implementation can be changed by setting the value of the
        /// LoginConfigurationProvider key of the AppSettings node of the application configuration file
        /// to the fully qualified name of the desired ILoginConfiguration subclass implementation
        /// </remarks>
        public static ILoginConfiguration Instance
        {
            get { return instance_; }
        }

        /// <summary>
        /// Retrieve the LoginModuleEntry for the specified name
        /// </summary>
        /// <param name="name">the name used to index the module</param>
        /// <returns>A LoginModuleEntry for the spcified <paramref name="name"/>, or null if there are no
        /// entry for the specified <paramref name="name"/></returns>
        public ILoginModuleEntry GetLoginModuleEntry(string name)
        {
            return base.LoginModules[name] as ILoginModuleEntry;
        }

        /// <summary>
        /// Gets all the login modules configured for the application.
        /// </summary>
        /// <returns>An array of LoginModuleEntry containg all the login
        /// modules configured for the application</returns>
        public new ILoginModuleEntry[] LoginModules
        {
            get {
                DictionaryValue modules = base.LoginModules;
                return modules.ToArray<ILoginModuleEntry>();
            }
        }
    }
}