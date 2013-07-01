using System;
using System.Collections.Generic;

using Nohros.Configuration;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// A factory used to create instance of the <see cref="ILoginModule"/> class.
  /// </summary>
  public interface ILoginModuleFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="ILoginModule"/> class.
    /// </summary>
    /// <param name="subject">
    /// The <see cref="Subject"/> to be authenticated.
    /// </param>
    /// <param name="callback">
    /// A <see cref="IAuthCallbackHandler"/> for communicating with the end
    /// user.
    /// </param>
    /// <param name="shared_state">
    /// State shared with other configured <see cref="ILoginModule"/>s.
    /// </param>
    /// <param name="login_module_node">
    /// A <see cref="ILoginModuleNode"/> containing information about the
    /// login module that should be instantiated.
    /// </param>
    ILoginModule CreateLoginModule(Subject subject,
      IAuthCallbackHandler callback, IDictionary<string, object> shared_state,
      ILoginModuleNode login_module_node);
  }
}
