using System;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class LoginModuleNode
  {
    /// <summary>
    /// Parses the <see cref="XmlElement"/> element into a
    /// <see cref="LoginModuleNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the login module configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the login module's location.
    /// </param>
    /// <returns>
    /// A <see cref="LoginModuleNode"/> containing the configured login module.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element"/> is a <c>null</c> reference.
    /// </exception>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    public static LoginModuleNode Parse(XmlElement element,
      string base_directory) {
      CheckPreconditions(element, base_directory);

      string name = GetAttributeValue(element, Strings.kNameAttribute);
      string type = GetAttributeValue(element, Strings.kTypeAttribute);
      string location = GetLocation(element, base_directory);
      LoginModuleControlFlag control_flag = GetControlFlag(element);

      LoginModuleNode login_module = new LoginModuleNode(name, type,
        control_flag, location);
      login_module.options = GetOptions(element);
      return login_module;
    }

    static LoginModuleControlFlag GetControlFlag(XmlElement element) {
      string control_flag_string =
        GetAttributeValue(element, Strings.kControlFlagAttribute).ToLower();

      if (control_flag_string == "required") {
        return LoginModuleControlFlag.Required;
      }

      if (control_flag_string == "requisite") {
        return LoginModuleControlFlag.Requisite;
      }
      if (control_flag_string == "sufficient") {
        return LoginModuleControlFlag.Sufficient;
      }
      if (control_flag_string == "optional") {
        return LoginModuleControlFlag.Optional;
      }

      throw new ConfigurationException(
        string.Format(
          Resources.Resources.Configuration_InvalidLoginModuleControlFlag,
          control_flag_string));
    }
  }
}
