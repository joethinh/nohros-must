using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// This class represents a single login module configured for the application specified
    /// in the <see cref="ILoginConfiguration.GetLoginModuleEntry(String)"/> method in the
    /// ILoginConfiguration class. Each respective LoginEntryModule contains a login module's name,
    /// and Type, a control flag( LoginModuleControlFlag ), and a login module's specific options.
    /// </summary>
    /// <seealso cref="ILoginConfiguration"/>
    public class LoginModuleEntry
    {
        #region LoginModuleControlFlag
        /// <summary>
        /// Represents whether or not a ILoginModule implementation class is REQUIRED, REQUISITE,
        /// SUFFICIENT or OPTIONAL.
        /// </summary>
        public enum LoginModuleControlFlag: byte
        {
            /// <summary>
            /// Optional login module
            /// </summary>
            OPTIONAL = 0,

            /// <summary>
            /// Required login module
            /// </summary>
            REQUIRED = 1,

            /// <summary>
            /// Requisite login module
            /// </summary>
            REQUISITE = 2,

            /// <summary>
            /// Suficient login module
            /// </summary>
            SUFFICIENT = 3
        }
        #endregion

        string _loginModuleName;
        LoginModuleControlFlag _controlFlag;
        IDictionary<string, object> _options;
        Type _loginModuleType;
        ILoginModule _module;

        #region .ctor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="loginModuleName">A string representing the class name of the login module
        /// configured for the specified application</param>
        /// <param name="loginModuleType">the Type of the login module class</param>
        /// <param name="controlFlag">either REQUIRED, REQUISITE, SUFICCIENT or OPTIONAL</param>
        /// <param name="options">the options configured for this login module</param>
        public LoginModuleEntry(string loginModuleName, Type loginModuleType, LoginModuleControlFlag controlFlag, IDictionary<string, object> options)
        {
            if (loginModuleName == null)
                throw new ArgumentNullException("loginModule");

            if (loginModuleName.Length == 0)
                throw new ArgumentException("loginModule");

            if (controlFlag < LoginModuleControlFlag.OPTIONAL || controlFlag > LoginModuleControlFlag.SUFFICIENT)
                throw new ArgumentOutOfRangeException("controlFlag");

            _loginModuleName = loginModuleName;
            _controlFlag = controlFlag;
            _options = options;
            _loginModuleType = loginModuleType;
        }
        #endregion

        /// <summary>
        /// Gets the name of the login module
        /// </summary>
        public string LoginModuleName
        {
            get { return _loginModuleName; }
        }


        internal ILoginModule Module
        {
            get
            {
                if (_module == null)
                    _module = ModuleActivator.GetInstance(_loginModuleType);
                return _module;
            }
        }

        /// <summary>
        /// Gets the type of the login module class.
        /// </summary>
        public Type LoginModuleType
        {
            get { return _loginModuleType; }
        }

        /// <summary>
        /// Gets the control flag for this login module.
        /// </summary>
        public LoginModuleControlFlag ControlFlag
        {
            get { return _controlFlag; }
        }

        /// <summary>
        /// Gets the options configured for this login module.
        /// </summary>
        public IDictionary<string, object> Options
        {
            get { return _options; }
        }
    }
}
