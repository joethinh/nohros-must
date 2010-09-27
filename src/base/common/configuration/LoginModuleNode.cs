using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Nohros.Security.Auth;
using Nohros.Resources;

namespace Nohros.Configuration
{
    public class LoginModuleNode : ConfigurationNode
    {
        internal const string kNodeTree = CommonNode.kNodeTree + CommonNode.kLoginModulesNodeName + ".";

        const string kTypeAttributeName = "type";
        const string kFlagAttributeName = "flag";

        Dictionary<string, object> options_;
        LoginModuleControlFlag control_flag_;
        Type type_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the LoginModuleNode
        /// </summary>
        public LoginModuleNode(string name, ConfigurationNode parent_node): base(name, parent_node) { }
        #endregion

        /// <summary>
        /// Parses a XML node that contains information about a login module.
        /// </summary>
        /// <param name="node">The XML node to parse.</param>
        /// <exception cref="ConfigurationErrosException">The <paramref name="node"/> is not a
        /// valid representation of a login module.</exception>
        public override void Parse(XmlNode node) {
            string type, flag;
            if (!GetAttributeValue(node, kTypeAttributeName, out type))
                Thrower.ThrowConfigurationException(StringResources.Auth_Config_Missing_LoginModuleType);

            // attempt to get the .NET type from the type string.
            type_ = Type.GetType(type);
            if (type_ == null)
                Thrower.ThrowConfigurationException(StringResources.Auth_Config_InvalidModuleType);

            if (!GetAttributeValue(node, kFlagAttributeName, out flag))
                Thrower.ThrowConfigurationException(StringResources.Auth_Config_Missing_ControlFlag);

            flag = flag.ToLower();

            // parse the control flag
            if(flag == "required")
                control_flag_ = LoginModuleControlFlag.REQUIRED;
            else if(flag == "requisite")
                control_flag_ = LoginModuleControlFlag.REQUIRED;
            else if(flag =="sufficient")
                control_flag_ = LoginModuleControlFlag.REQUIRED;
            else if(flag == "optional")
                control_flag_ = LoginModuleControlFlag.REQUIRED;
            else
                Thrower.ThrowConfigurationException(StringResources.Auth_Config_InvalidControlFlag);

            options_ = GetOptions(node);
        }

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
    }
}