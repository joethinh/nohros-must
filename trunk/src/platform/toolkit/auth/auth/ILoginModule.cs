using System;
using Nohros.Configuration;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// The <see cref="ILoginModule"/> interface describes the methods and
  /// properties that should be implemented by authentication technology
  /// providers. <see cref="ILoginModule"/> objects are plugged in under
  /// applications to provide a particular type of authentication.
  /// <para>
  /// While applications write to the <see cref="LoginContext"/> API,
  /// authentication technology providers implements the
  /// <see cref="ILoginModule"/> interface. A configuration file specifies the
  /// login module(s) to be used with a particular login application. Therefore
  /// different login modules can be plugged in under the application without
  /// requiring any modifications to the application itself.
  /// </para>
  /// <para>
  /// The calling application sees the authentication process as a single
  /// operation. However, the authentication process whitin the login module
  /// proceeds in two distinct phases. In the first phase, the login module's
  /// <see cref="ILoginModule.Login()"/> method gets invoked by the
  /// LoginContext's <see cref="LoginContext.Login()"/> method. The login
  /// method for the login module's then performs the actual authentication
  /// and saves its authentication status as private state information. Once
  /// finished, the login module's either returns true (if it succeeded) or
  /// false (if it failed). In the failure case, the login module's must not
  /// retry the authentication or introduce delays. The responsability of such
  /// tasks belongs to the application. If the application attempts to retry
  /// the authentication, the login module's login method will be called
  /// again.
  /// </para>
  /// <para>
  /// In the second phase, if the <see cref="LoginContext"/>'s overall
  /// authentication succeeded (the relevant "Required", "Requisite",
  /// "Sufficient" and "Optional" login module succeeded), then the
  /// <see cref="ILoginModule.Commit"/> method for the login module gets
  /// invoked. The commit method for a login module checks its privately saved
  /// state to see if its own authentication succeeded. If the overall
  /// <see cref="LoginContext"/> authentication succeeded and the login
  /// module's own authentication succeeded, then the commit method
  /// associates the relevant credentials (authentication data) with the
  /// <see cref="ISubject"/> located within the login module.
  /// </para>
  /// <para>
  /// If the <see cref="LoginContext"/>'s overall authentication failed (the
  /// relevant "Required", "Requisite", "Sufficient" and "Optional" login
  /// module did not succeeded), then the <see cref="ILoginModule.Abort()"/>
  /// method for each login module gets invoked. In this case, the login module
  /// removes/destroy any authentication state originally saved.
  /// </para>
  /// <para>
  /// Log out involves only one phase. The <see cref="LoginContext"/> invokes
  /// the <see cref="ILoginModule.Logout()"/> method. The logout method
  /// for the login module then performs the logout procedures, such as logging
  /// session information.
  /// </para>
  /// </summary>
  public interface ILoginModule
  {
    /// <summary>
    /// Method to abort the authentication process (phase 2.)
    /// </summary>
    /// <remarks>
    /// This method is called if the <see cref="LoginContext"/> overall
    /// authentication failed.
    /// <para>
    /// If this <see cref="ILoginModule"/> own authentication attempt
    /// succeeded (checked by retrieving the private state saved by the
    /// <see cref="Login"/> method), then this method cleans up any state that
    /// was originally saved.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <c>true</c> if this method succeeded, or <c>false</c> if this
    /// <see cref="ILoginModule"/> should be ignored.
    /// </returns>
    /// <exception cref="LoginException">
    /// The <see cref="Login"/> operation fails.
    /// </exception>
    bool Abort(IAuthenticationInfo info);

    /// <summary>
    /// Method to commit the authentication process (phase 2).
    /// <para>
    /// This method is called by the <see cref="LoginContext"/> if overall
    /// authentication succeeded.
    /// </para>
    /// <para>
    /// If this <see cref="ILoginModule"/> own authentication attempt succeeded
    /// (checked by retrieving the private state saved by the
    /// <see cref="Login"/> method), ths this method cleans up any saved state
    /// that was originally saved.
    /// </para>
    /// </summary>
    /// <returns>
    /// <c>true</c> if this method succeeded, or <c>false</c> if this
    /// <see cref="ILoginModule"/> should be ignored.
    /// </returns>
    /// <exception cref="LoginException">
    /// The <see cref="Commit"/> operation fails.
    /// </exception>
    bool Commit(IAuthenticationInfo info);

    /// <summary>
    /// Method to authenticate a subject.
    /// </summary>
    /// <remarks>
    /// The implementation of this method authenticates a <see cref="ISubject"/>.
    /// For exemple, it may prompt for <see cref="ISubject"/> information
    /// such as username and password and then attempt to verify the password.
    /// This method saves the result of authentication attempt as private
    /// state within a <see cref="IAuthenticationInfo"/> object.
    /// </remarks>
    /// <returns>
    /// <c>true</c> if the authentication succeeded, or <c>false</c> if this
    /// <see cref="ILoginModule"/> should be ignored.
    /// </returns>
    /// <exception cref="LoginException">
    /// The <see cref="Login"/> operation fails.
    /// </exception>
    IAuthenticationInfo Login();

    /// <summary>
    /// Method which logs out a <see cref="ISubject"/>.
    /// </summary>
    /// <exception cref="LoginException">
    /// The <see cref="Logout"/> operation fails.
    /// </exception>
    bool Logout(ISubject subject);

    /// <summary>
    /// Gets the login's module control flag.
    /// </summary>
    /// <value>
    /// A <see cref="LoginModuleControlFlag"/> specifying whether this login
    /// module is Required, Requisite, Sufficient or Optional.
    /// </value>
    LoginModuleControlFlag ControlFlag { get; }
  }
}
