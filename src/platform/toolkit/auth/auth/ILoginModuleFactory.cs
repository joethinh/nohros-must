using System;
using System.Collections.Generic;

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
    /// <param name="options">
    /// A collection of key/value pair containing the user defined options for
    /// the login module that should be created.
    /// </param>
    /// <param name="shared">
    /// A collection of key/value pair containing the state shared with other
    /// <see cref="ILoginModule"/>.
    /// </param>
    /// <param name="callback">
    /// A <see cref="IAuthCallbackHandler"/> for communicating with the end
    /// user (prompting for usernames and passwords, for example).
    /// </param>
    /// <remarks>
    /// This method is used by a <see cref="LoginContext"/> object to create
    /// a instance of the <see cref="ILoginModule"/> class. If thislogin module
    /// does not understand any of the data stored in the
    /// <paramref name="shared"/> or <paramref name="options"/> parameters,
    /// they can be ignored.
    /// </remarks>
    ILoginModule CreateLoginModule(
      IAuthCallbackHandler callback,
      IDictionary<string, string> shared,
      IDictionary<string, string> options);
  }
}
