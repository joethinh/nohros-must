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
    /// A collection of key/value pair contained the user defined options for
    /// the login module that should be created.
    /// </param>
    ILoginModule CreateLoginModule(IDictionary<string, string> options);
  }
}
