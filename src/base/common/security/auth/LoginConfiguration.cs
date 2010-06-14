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
    public class LoginConfiguration : ILoginConfiguration
    {
        IConfigurationImpl _config;

        #region .ctor
        public LoginConfiguration()
        {
            _config = new IConfigurationImpl();
            _config.Load();
        }
        #endregion

        /// <summary>
        /// Retrieve the LoginModuleEntry for the specified name
        /// </summary>
        /// <param name="name">the name used to index the module</param>
        /// <returns>A LoginModuleEntry for the spcified <paramref name="name"/>,
        /// or null if there are no entry for the specified <paramref name="name"/></returns>
        public override LoginModuleEntry this[string name]
        {
            get { return _config.GetLoginModuleEntry(name); }
        }

        /// <summary>
        /// Gets all the login modules configured for the application.
        /// </summary>
        /// <returns>An array of LoginModuleEntry containg all the login
        /// modules configured for the application</returns>
        public override LoginModuleEntry[] LoginModules
        {
            get { return _config.ModuleEntries; }
        }
    }
}