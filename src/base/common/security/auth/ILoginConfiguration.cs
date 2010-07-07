    using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// A configuration object is responsible for specifiying which login modules should be
    /// used for a particular application, and what order the login modules should be invoked.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each module is indexed by its name. Each type property of each login module entry must
    /// contain the fully qualified class name. Authentication proceeds down the module list in
    /// the exact order specified.
    /// </para>
    /// <para>
    /// The flag value controls the overall behavior as authentication proceeds down the stack.
    /// The following represents a description of the valid valus for <see cref="LoginModuleControlFlag"/>
    /// and their respective semantics:
    /// <para>
    ///     Required    -   The login module is required to succeed. If it succeeds or fails, authentication
    ///                     still continues to proceed down the login module list
    /// </para>
    /// <para>
    ///     Requisite   -   The login module is required to succeed. If it is succeeds, authentication
    ///                     continues down the login module list.If it fails, control immediately returns
    ///                     to the application( authentication does not procced down the login module list).
    /// </para>
    /// <para>
    ///     Sufficient  -   The login module is not required to succeed. If it does succeed, control
    ///                     immediately returns to the application (authentication does not procced
    ///                     down the login module list). If it fails, authentication continues down
    ///                     the login module list.
    /// </para>
    /// <para>
    ///     Optional    -   The login module is not required to succed. If it succeeds or fails,
    ///                     authentication still continues to procced down the login module list.
    /// </para>
    /// </para>
    /// <para>
    /// The overall authentication succeeds only if all Required and Requisite login modules succeed.
    /// If a sufficient login module is configured and succeeds, then only the Required and Requisite
    /// login modules prior to that Sufficient login module need to have succeeded for the overall
    /// authentication to succeed. If no Required or Requisite login module are configured for an
    /// application, then at least one Sufficient or Optional login module must secceed.
    /// </para>
    /// <para>
    /// Module options is a collection of XML nodes containing the login module specific values which 
    /// are passed directly to the underlying login modules. Options are defined by the login module itself,
    /// and control the behavior within it. For example, a login module may define options to support
    /// debugging/testing capabilities. The default way to specify options in the configuration is by
    /// using the following syntax: <optionname>optionvalue</optionname>. The key must be a child node
    /// of a the module node and the value should be the value of that node. If a string in the
    /// form ${system.enviroment.property}, occurs in the value, it will be expanded to the value of the
    /// system property. Note that there is no limit to the number of options a login module may define.
    /// </para>
    /// <para>
    /// The following represents an example of configuration entry on the syntax above:
    /// <example>
    ///     ...
    ///     ...
    ///     <MyLoginModule
    ///         type="MyNamespace.MyLoginModule, Nohros"
    ///         flag="required">
    ///         <useTicketCache>true</useTicketCache>
    ///         <ticketCache>${System.Environment.CurrentDirectory}</ticketCache>
    ///     </MyLoginModule>
    ///     ...
    ///     ...
    /// </example>
    /// This configuration specifies that the application requires users to authenticate to the
    /// MyNamespace.MyLoginModule, which is required to succeed. Also note that the login 
    /// module specific options, useTicketCache="true" and ticketCache="${System.Enviroment.UserName}",
    /// are passed to the CommomLoginModule. These options instruct the MyLoginModule to use the ticket
    /// cache at the specified location. The system property System.Enviroment.CurrentDirectory is
    /// expanded to it respective value.
    /// </para>
    /// <para>
    /// There is only one <see cref="ILoginConfiguration"/> object instantiated in the current
    /// AppDomain at any given time. A ILoginConfiguration object can be set the LoginConfiguration
    /// property of the ILoginConfiguration.
    /// </para>
    /// <para>
    /// If no ILoginConfiguration object has been instantiated in the AppDomain, a call to 
    /// <see cref="ILoginConfiguration.LoginConfiguration()"/> instantiate the default LoginConfiguration
    /// implementation(a default subclass implementation of this abstract class). The default
    /// LoginConfiguration subclass implementation can be changed by setting the value of the
    /// LoginConfigurationProvider key of the AppSettings node of the application configuration file
    /// to the fully qualified name of the desired ILoginConfiguration subclass implementation. The
    /// application configuration file is either <c>MyAppNAme.exe.config</c> for a normal application
    /// or <c>Web.config</c> for an ASP.NET application.
    /// </para>
    /// </remarks>
    public abstract class ILoginConfiguration
    {
        static ILoginConfiguration _lognConfig = null;
        static object _syncLock = new object();

        /// <summary>
        /// Sole constructor
        /// </summary>
        protected ILoginConfiguration()
        {
        }

        /// <summary>
        /// Gets or sets the single instance of the ILoginConfiguration class.
        /// </summary>
        /// <returns>A instance of the ILoginConfiguration class</returns>
        /// <remarks>
        /// The default LoginConfiguration implementation can be changed by setting the value of the
        /// LoginConfigurationProvider key of the AppSettings node of the application configuration file
        /// to the fully qualified name of the desired ILoginConfiguration subclass implementation
        /// </remarks>
        public static ILoginConfiguration LoginConfiguration
        {
            get
            {
                if (_lognConfig == null)
                {
                    lock (_syncLock)
                    {
                        if (_lognConfig == null)
                        {
                            Type type;

                            string providerName = ConfigurationManager.AppSettings["LoginConfigurationProvider"] as string;
                            if (providerName != null)
                            {
                                type = Type.GetType(providerName);
                                if (type != null)
                                {
                                    try
                                    {
                                        _lognConfig = (LoginConfiguration)Activator.CreateInstance(type);
                                    }
                                    catch { _lognConfig = null; }
                                }
                            }

                            // The specified provider could not be loaded or a custom provider
                            // was not specified. We will load the default configuration provider
                            if (_lognConfig == null)
                            {
                                _lognConfig = new LoginConfiguration();
                            }
                        }
                    }
                }
                return _lognConfig;
            }
            set
            {
                lock(_syncLock)
                {
                    _lognConfig = value;
                }
            }
        }

        /// <summary>
        /// Retrieve the LoginModuleEntry for the specified name
        /// </summary>
        /// <param name="name">the name used to index the module</param>
        /// <returns>A LoginModuleEntry for the spcified <paramref name="name"/>,
        /// or null if there are no entry for the specified <paramref name="name"/></returns>
        public abstract LoginModuleEntry this[string key] { get; }

        /// <summary>
        /// Gets all the login modules configured for the application.
        /// </summary>
        /// <returns>An array of LoginModuleEntry containg all the login
        /// modules configured for the application</returns>
        public abstract LoginModuleEntry[] LoginModules { get; }
    }
}
