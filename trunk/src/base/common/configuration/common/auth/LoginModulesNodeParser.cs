using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Nohros.Configuration
{
  public partial class LoginModulesNode
  {
    /// <summary>
    /// Parses the specified <see cref="XmlElement"/> element into a
    /// <see cref="LoginModulesNode"/> object.
    /// </summary>
    /// <param name="element">
    /// A Xml element that contains the login module configuration data.
    /// </param>
    /// <param name="base_directory">
    /// The base directory to use when resolving the login module's location.
    /// </param>
    /// <exception cref="ConfigurationException">
    /// The <paramref name="element"/> contains invalid configuration data.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// <paramref name="base_directory"/> does not exists.
    /// </exception>
    public static LoginModulesNode Parse(XmlElement element, string base_directory) {
      List<LoginModuleNode> login_module_nodes = new List<LoginModuleNode>();
      foreach (XmlNode node in element.ChildNodes) {
        if (node.NodeType == XmlNodeType.Element &&
          Strings.AreEquals(node.Name, Strings.kLoginModuleNodeName)) {
          login_module_nodes.Add(
            LoginModuleNode.Parse(element, base_directory));
        }
      }
      LoginModulesNode login_modules_node = new LoginModulesNode();
      login_modules_node.login_module_nodes_ = login_module_nodes.ToArray();
      return login_modules_node;
    }
  }
}
