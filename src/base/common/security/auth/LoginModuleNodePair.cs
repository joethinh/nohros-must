using System;
using Nohros.Configuration;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Provides a basic implementation od the <see cref="ILoginModuleNodePair"/>
  /// interface.
  /// </summary>
  public class LoginModuleNodePair : ILoginModuleNodePair
  {
    readonly ILoginModule login_module_;
    readonly ILoginModuleNode login_module_node_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModuleNodePair"/>
    /// class by using the specified login module node and login module.
    /// </summary>
    /// <param name="login_module_node">
    /// A <see cref="ILoginModuleNode"/> that contains information about the
    /// associated login module.
    /// </param>
    /// <param name="login_module">
    /// A <see cref="ILoginModule"/> that is associated with the given
    /// <paramref name="login_module_node"/>.
    /// </param>
    public LoginModuleNodePair(ILoginModuleNode login_module_node,
      ILoginModule login_module) {
      login_module_ = login_module;
      login_module_node_ = login_module_node;
    }
    #endregion

    #region ILoginModuleNodePair Members
    /// <inheritdoc/>
    public ILoginModule LoginModule {
      get { return login_module_; }
    }

    /// <inheritdoc/>
    public ILoginModuleNode LoginModuleNode {
      get { return login_module_node_; }
    }
    #endregion
  }
}
