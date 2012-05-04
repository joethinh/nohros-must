using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Configuration
{
  /// <summary>
  /// A <see cref="LoginModulesNode"/> is a collection of
  /// <see cref="LoginModuleNode"/>.
  /// </summary>
  public partial class LoginModulesNode : AbstractConfigurationNode, ILoginModulesNode
  {
    ILoginModuleNode[] login_module_nodes_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModulesNode"/>.
    /// </summary>
    public LoginModulesNode() : base(Strings.kLoginModulesNodeName) {
      login_module_nodes_ = new ILoginModuleNode[0];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModulesNode"/> class
    /// by using the specified array of login module node.
    /// </summary>
    /// <param name="login_module_nodes">
    /// An array of <see cref="LoginModuleNode"/> that contains the login
    /// modules configured for this application.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="login_module_nodes"/> is <c>null</c>.
    /// </exception>
    public LoginModulesNode(ILoginModuleNode[] login_module_nodes) : this() {
      if (login_module_nodes == null) {
        throw new ArgumentNullException("login_module_nodes");
      }
      login_module_nodes_ = login_module_nodes;
    }
    #endregion

    /// <summary>
    /// Gets a <see cref="LoginModuleNode"/> node whose name is
    /// <paramref name="login_module_name"/>.
    /// </summary>
    /// <param name="login_module_name">
    /// The name of the login module.
    /// </param>
    /// <returns>
    /// A <see cref="LoginModuleNode"/> object whose name is
    /// <paramref name="login_module_name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// A login module whose name is <param name="login_module_name"> was
    /// not found.
    /// </param>
    /// </exception>
    public ILoginModuleNode GetLoginModuleNode(string login_module_name) {
      ILoginModuleNode login_module_node;
      if (GetLoginModuleNode(login_module_name, out login_module_node)) {
        return login_module_node;
      }
      throw new KeyNotFoundException(login_module_name);
    }

    /// <summary>
    /// Gets a <see cref="LoginModuleNode"/> whose name is
    /// <paramref name="login_module_name"/>.
    /// </summary>
    /// <param name="login_module_name">
    /// The name of the login module.
    /// </param>
    /// <param name="login_module_node">
    /// When this method returns contains a <see cref="LoginModuleNode"/>
    /// object whose name is <paramref name="login_module_name"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> when a login module whose name is
    /// <paramref name="login_module_name"/> is found; otherwise, <c>false</c>.
    /// </returns>
    public bool GetLoginModuleNode(string login_module_name,
      out ILoginModuleNode login_module_node) {
      for (int i = 0, j = login_module_name.Length; i < j; i++) {
        login_module_node = login_module_nodes_[i];
        if (StringsAreEquals(login_module_name, login_module_node.Name)) {
          return true;
        }
      }
      login_module_node = null;
      return false;
    }

    /// <summary>
    /// Gets an array containing all the configured login modules.
    /// </summary>
    /// <remarks>
    /// If no login modules was configured for the application, this method
    /// returns an empty array.
    /// </remarks>
    public ILoginModuleNode[] LoginModuleNodes {
      get { return login_module_nodes_; }
    }
  }
}
