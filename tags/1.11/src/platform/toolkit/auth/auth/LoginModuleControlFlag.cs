using System;
using Nohros.Security.Auth;

namespace Nohros.Configuration
{
  /// <summary>
  /// Represents whether or not a <see cref="ILoginModule"/> implementation
  /// class is Required, Requisite, Sufficient or Optional.
  /// </summary>
  public enum LoginModuleControlFlag
  {
    /// <summary>
    /// The login module is not required to succed. If it succeeds or
    /// fails, authentication still continues to procced down the login module
    /// list.
    /// </summary>
    Optional = 0,

    /// <summary>
    /// The login module is required to succeed. If it succeeds or
    /// fails, authentication still continues to proceed down the login module
    /// list.
    /// </summary>
    Required = 1,

    /// <summary>
    /// The login module is required to succeed. If it is succeeds,
    /// authentication continues down the login module list. If it fails,
    /// control immediately returns to the application (authentication does not
    /// procced down the login module list).
    /// </summary>
    Requisite = 2,

    /// <summary>
    /// The login module is not required to succeed. If it does
    /// succeed, control immediately returns to the application(
    /// authentication does not procced down the login module list). If it
    /// fails, authentication continues down the login module list.
    /// </summary>
    Sufficient = 3
  }
}
