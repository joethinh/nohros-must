using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Nohros.Configuration;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A LoginConfigur
    /// </summary>
    public class LoginConfiguration : NohrosConfiguration, ILoginConfiguration
    {
        static LoginConfiguration instance;

        LoginModuleNode[] modules_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the LoginConfiguration class.
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
            instance = new LoginConfiguration();
            instance.Load();

            // we need to store a reference to all nodes configured for a specific application.
            // this will be stored into a array for faster retrieval.
            if (instance.CommonNode != null) {
                List<ConfigurationNode> nodes = instance.CommonNode.ChildNodes;
                if (nodes != null && nodes.Count > 0) {
                    List<LoginModuleNode> modules = new List<LoginModuleNode>(nodes.Count);
                    foreach (LoginModuleNode module in modules) {
                        modules.Add(module);
                    }
                    instance.modules_ = modules.ToArray();
                }
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the single instance of the ILoginConfiguration class.
        /// </summary>
        /// <returns>A instance of the ILoginConfiguration class</returns>
        /// <remarks>
        /// The default LoginConfiguration implementation can be changed by setting the value of the
        /// LoginConfigurationProvider key of the AppSettings node of the application configuration file
        /// to the fully qualified name of the desired ILoginConfiguration subclass implementation
        /// </remarks>
        public static ILoginConfiguration Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Retrieve the LoginModuleEntry for the specified name
        /// </summary>
        /// <param name="name">the name used to index the module</param>
        /// <returns>A LoginModuleEntry for the spcified <paramref name="name"/>, or null if there are no
        /// entry for the specified <paramref name="name"/></returns>
        public ILoginModuleEntry GetLoginModuleEntry(string name)
        {
            return CommonNode.GetLoginModule(name);
        }

        /// <summary>
        /// Gets all the login modules configured for the application.
        /// </summary>
        /// <returns>An array of LoginModuleEntry containg all the login
        /// modules configured for the application</returns>
        public ILoginModuleEntry[] LoginModules
        {
            get { return modules_; }
        }
    }
}