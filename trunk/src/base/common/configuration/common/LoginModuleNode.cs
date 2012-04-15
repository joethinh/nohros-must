using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Configuration;

using Nohros.Logging;
using Nohros.Security.Auth;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class LoginModuleNode : AbstractConfigurationNode, ILoginModuleEntry
    {
        const string kTypeAttributeName = "type";
        const string kFlagAttributeName = "flag";

        Dictionary<string, object> options_;
        LoginModuleControlFlag control_flag_;
        Type type_;
        ILoginModule module_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the LoginModuleNode
        /// </summary>
        public LoginModuleNode(string name): base(name) { }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a login module.
        /// </summary>
        /// <param name="node">A XML node containing the data to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a login module.</exception>
        public void Parse(XmlNode node) {
            string type, flag;
            if (!GetAttributeValue(node, kTypeAttributeName, out type))
                throw new ConfigurationErrorsException(StringResources.Auth_Config_Missing_LoginModuleType);

            // attempt to get the .NET type from the type string.
            type_ = Type.GetType(type);
            if (type_ == null)
                throw new ConfigurationErrorsException(string.Format(StringResources.Auth_Config_InvalidModuleType, type));

            if (!GetAttributeValue(node, kFlagAttributeName, out flag))
                throw new ConfigurationErrorsException(StringResources.Auth_Config_Missing_ControlFlag);

            flag = flag.ToLower();

            // parse the control flag
            if(flag == "required")
                control_flag_ = LoginModuleControlFlag.REQUIRED;
            else if(flag == "requisite")
                control_flag_ = LoginModuleControlFlag.REQUISITE;
            else if(flag =="sufficient")
                control_flag_ = LoginModuleControlFlag.SUFFICIENT;
            else if(flag == "optional")
                control_flag_ = LoginModuleControlFlag.OPTIONAL;
            else
                throw new ConfigurationErrorsException(StringResources.Auth_Config_InvalidControlFlag);

            options_ = GetOptions(node);
        }

        #region ILoginModuleEntry
        static Dictionary<string, object> GetOptions(XmlNode node)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();

            foreach (XmlNode n in node) {
                string name = n.Name, value = n.InnerText, val = null;
                if (value.StartsWith("${")) {
                    try {
                        val = value;
                        value = System.Environment.ExpandEnvironmentVariables(value.Substring(3, value.Length - 3));
                    } catch { value = val; }
                }
                options.Add(name, value);
            }
            return options;
        }

        /// <summary>
        /// Gets the related login module.
        /// </summary>
        public ILoginModule Module {
            get {
                if (module_ == null) {
                    try {
                        Interlocked.CompareExchange<ILoginModule>(ref module_, Activator.CreateInstance(type_) as ILoginModule, null);
                    } catch(Exception exception) {
                        MustLogger.ForCurrentProcess.Error("[Module   Nohros.COnfiguration.LoginModuleNode]", exception);
                        module_ = null;
                    }
                }
                return module_;
            }
        }

        /// <summary>
        /// Gets the type of the login module class.
        /// </summary>
        public Type Type {
            get { return type_; }
        }

        /// <summary>
        /// Gets the control flag for this login module.
        /// </summary>
        public LoginModuleControlFlag ControlFlag {
            get { return control_flag_; }
        }

        /// <summary>
        /// Gets the options configured for this login module.
        /// </summary>
        public IDictionary<string, object> Options {
            get { return options_; }
        }
        #endregion
    }
}