using System;
using System.Collections.Generic;
using Nohros.Security.Auth;

namespace Nohros.Security
{
  /// <summary>
  /// A factory for the <see cref="LoginContext"/> class.
  /// </summary>
  public class LoginContextFactory
  {
    readonly IEnumerable<ILoginModuleFactory> login_module_factories_;

    #region .ctor
    public LoginContextFactory(
      IEnumerable<ILoginModuleFactory> login_module_factories) {
      login_module_factories_ = login_module_factories;
    }
    #endregion

    public LoginContext CreateLoginContext(Subject subject) {
    }

    public LoginContext CreateLoginContext(Subject subject,
      IAuthCallbackHandler callback) {
    }

    public LoginContext CreateLoginContext(IAuthCallbackHandler callback) {
    }
  }
}
