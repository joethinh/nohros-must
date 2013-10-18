using System;
using System.Collections.Generic;
using System.Linq;
using Nohros.Configuration;
using Nohros.Logging;
using Nohros.Resources;
using LoginModuleFactoryTuple =
  System.Collections.Generic.KeyValuePair
    <Nohros.Security.Auth.ILoginModuleFactory,
      System.Collections.Generic.IDictionary<string, string>>;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Describes the basic methods used to authenticate subjects and provides
  /// a way to develop an application independent of the underlying
  /// authentication technology.
  /// </summary>
  /// <remarks>
  /// The authentication technology is specified through the application
  /// configuration. So, different login  modules can be plugged in under an
  /// application without requiring any modifications to the application
  /// itself.
  /// <para>
  /// In addition to supporting pluggable authentication, this class also
  /// support the notion of stacked authentication. In other words, an
  /// application may be configured to use more than one
  /// <see cref="ILoginModule"/>. For example, one could configure both
  /// Kerberos login module and a smart card login module under an application.
  /// </para>
  /// <para>
  /// A typical caller instantiates this class and passes in a array of
  /// <see cref="ILoginModuleFactory"/> objects, which is used the
  /// to determine which login module should succeed in order for the overall
  /// authentication to succeed. The <see cref="IAuthCallbackHandler"/> object
  /// is passed to the underlying login modules so they may communicate and
  /// interact with users (prompting for a username and password via a
  /// graphical user interface, for example.
  /// </para>
  /// <para>
  /// Once the caller has instantiated a <see cref="LoginContext"/>, it
  /// invokes the <see cref="ILoginModule.Login"/> method for each login module
  /// of the specified array. Each login module then performs its respective
  /// type of authentication (username/password, smart card, pin verification,
  /// etc). Note that the login modules will not attempt authentication retries
  /// nor introduces delays if the authentication fails. Such tasks belong to
  /// the caller.
  /// </para>
  /// <para>
  /// Regardless of whether or not the overall authentication succeeded, this
  /// login method completes a 2-phase authentication process by then calling
  /// either the <see cref="ILoginModule.Commit()"/> method or the
  /// <see cref="ILoginModule.Abort()"/> method for each of the configured
  /// login modules. The commit method for each login module gets invoked if
  /// the overall authentication succeeded, whereas the abort method for each
  /// login module gets invoked if the overall authentication failed. Each
  /// successful login module's commit method associates the relevant
  /// permissions with the <see cref="AbstractSubject"/>. Each login module's abort
  /// method cleans up or removes/destroys any previously stored authentication
  /// state.
  /// </para>
  /// <para>
  /// If the login method returns without throwing an exception, then the
  /// overall authentication succeeded. The caller can then retrieve the
  /// newly authenticated <see cref="AbstractSubject"/> by getting the value of the
  /// <see cref="AbstractSubject"/> property. Permissions associated with the subject
  /// may be retrieved by getting the value associated with the subject
  /// respective <see cref="AbstractSubject.Permissions"/> property.
  /// </para>
  /// <para>
  /// To logout the subject, the caller simple needs to invoke the
  /// <see cref="Logout"/> method. As with the <see cref="Login"/> method, this
  /// logout method invokes the <see cref="ILoginModule.Logout"/> method for
  /// each login module configured for this login context. Each login module's
  /// logout method cleans up subject's state and removes/destroys their
  /// permissions as appropriate.
  /// </para>
  /// <para>
  /// Each of the configured login module invoked by the login context is
  /// initialized with a <see cref="AbstractSubject"/> object to be authenticated, a
  /// <see cref="IAuthCallbackHandler"/> object used to communicate with users,
  /// shared login module state, and login module specific options.
  /// </para>
  /// <para>
  /// Each login module which successfully authenticates a user updates the
  /// subject with the relevant user information (permissions). This subject
  /// can then be returned via the subject method from the
  /// <see cref="LoginContext"/> class if the overall authentication succeeds.
  /// </para>
  /// <para>
  /// A login context supports authentication retries by calling application.
  /// For example, a login context's login method may be invoked mutiple times
  /// if the user incorrectly types in a password. However, a login context
  /// should not be used to authenticate more than one subject. A separate
  /// login context should be used to authenticate each different subject.
  /// </para>
  /// <para>
  /// Mutiple calls into the same login context do not affect the login module
  /// state, or the login module specific options.
  /// </para>
  /// </remarks>
  /// <seealso cref="AbstractSubject"/>
  /// <seealso cref="IAuthCallbackHandler"/>
  /// <seealso cref="ILoginModule"/>
  /// <seealso cref="ILoginModuleFactory"/>
  public sealed class LoginContext
  {
    class ModuleAuthInfo
    {
      #region .ctor
      public ModuleAuthInfo(ILoginModule module,
        IAuthenticationInfo authentication_info) {
        Module = module;
        AuthenticationInfo = authentication_info;
      }
      #endregion

      public bool Commit() {
        return Module.Commit(AuthenticationInfo);
      }

      public bool Abort() {
        return Module.Abort(AuthenticationInfo);
      }

      public ILoginModule Module { get; private set; }
      public IAuthenticationInfo AuthenticationInfo { get; private set; }
    }

    readonly IList<ILoginModule> login_modules_;

    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="LoginContext"/> class by
    /// using the specified login modules.
    /// </summary>
    /// <param name="login_modules">
    /// A array containing all the configured login modules.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="login_modules"/> is <c>null</c>.
    /// </exception>
    public LoginContext(IEnumerable<ILoginModule> login_modules) {
      if (login_modules == null) {
        throw new ArgumentNullException("login_modules");
      }
      login_modules_ = login_modules.ToList();
    }
    #endregion

    /// <summary>
    /// Performs the authentication.
    /// </summary>
    /// <remarks>
    /// This method invokes the login method for each configured
    /// <see cref="ILoginModule"/>.
    /// Each <see cref="ILoginModule"/> then performs its respective type of
    /// authentication (username/password, smart card pin verification, etc.)
    /// <para>
    /// The method completes a 2-phase authentication process by calling each
    /// configured <see cref="ILoginModule.Commit"/> method if the overall
    /// authentication succeeded (the relevant Required, Requisite, Sufficient,
    /// and Optional <see cref="ILoginModule"/> succeeded, or by calling
    /// each configured <see cref="ILoginModule.Abort"/> method if the overall
    /// authentication failed. If the authentication succeeded, each successful
    /// <see cref="ILoginModule.Commit"/> method associates the relevant
    /// <see cref="IPermission"/> with the <see cref="AbstractSubject"/>.
    /// If authentication failed, each <see cref="ILoginModule.Abort"/> method
    /// removes/destroys any previous stored state.
    /// </para>
    /// <para>
    /// If the commit phase of the authentication process fails, then the
    /// overall authentication fails and this method invokes the abort method
    /// for each configured <see cref="ILoginModule"/>.
    /// </para>
    /// <para>
    /// If the abort method fails for any reason, the overall authentication
    /// fails.
    /// </para>
    /// <para>
    /// Note that if this method enters the abort phase (either the login or
    /// commit phase failed), this method invokes all <see cref="ILoginModule"/>
    /// configured for the application regardless of their respective
    /// configuration flag parameters. Essentially this means that required and
    /// sufficient semantics are ignored during the abort phase. This
    /// guarantees that proper cleanup and state restoration can take place.
    /// </para>
    /// </remarks>
    public bool Login(ISubject subject,
      IAuthCallbackHandler auth_callback_handler) {
      bool overall_login_succeeds = true;

      var attempted_login_modules =
        new List<ModuleAuthInfo>(login_modules_.Count);

      foreach (ILoginModule login_module in login_modules_) {
        // A try/catch block is used here to ensure that the login method will
        // be called for each configured module (respecting the control flag
        // semantics).
        IAuthenticationInfo auth_info;
        try {
          auth_info = login_module.Login(auth_callback_handler);
          attempted_login_modules.Add(
            new ModuleAuthInfo(login_module, auth_info));
        } catch (Exception ex) {
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "login"), ex);
          auth_info = AuthenticationInfos.Failed();
        }

        if (auth_info.Authenticated) {
          if (login_module.ControlFlag == LoginModuleControlFlag.Sufficient) {
            break;
          }
        } else {
          // The login has failed, if the failed module is "requisite" or
          // "required" the overall login should fail.
          if (login_module.ControlFlag == LoginModuleControlFlag.Requisite) {
            // If the failed module is "requisite" we need to stop procceding
            // down the login module list.
            overall_login_succeeds = false;
            break;
          }

          // If the failed module is "required", authentication should still
          // continues to proceed down the login module list.
          if (login_module.ControlFlag == LoginModuleControlFlag.Required) {
            overall_login_succeeds = false;
          }
        }
      }

      if (!overall_login_succeeds) {
        Abort(attempted_login_modules);
        return false;
      }

      try {
        if (!Commit(attempted_login_modules)) {
          Abort(attempted_login_modules);
          return false;
        }
      } catch (Exception e) {
        MustLogger.ForCurrentProcess.Debug(string.Format(
          StringResources.Log_ThrowsException, "commit"), e);
      }

      return true;
    }

    /// <summary>
    /// Execute the commit method for the login modules that has succeed.
    /// </summary>
    /// <param name="attempted_login_modules"></param>
    /// <returns></returns>
    bool Commit(IEnumerable<ModuleAuthInfo> attempted_login_modules) {
      return
        attempted_login_modules.All(
          module_auth_info => module_auth_info.Commit());
    }

    /// <summary>
    /// Call the <see cref="ILoginModule.Abort"/> method for all configured
    /// login modules.
    /// </summary>
    /// <remarks>
    /// If a <see cref="ILoginModule"/> throws an exception, it will be logged
    /// and we proceed down the list of login modules.
    /// </remarks>
    void Abort(IEnumerable<ModuleAuthInfo> attempted_login_modules) {
      foreach (ModuleAuthInfo module_auth_info in attempted_login_modules) {
        try {
          module_auth_info.Abort();
        } catch (Exception exception) {
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "Abort"), exception);
        }
      }
    }

    /// <summary>
    /// Logs the <see cref="AbstractSubject"/> out, cleaning up any state that may be
    /// in memory.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This method invokes the logout method for each
    /// <see cref="ILoginModule"/> configured for this
    /// <see cref="LoginContext"/>. Each <see cref="ILoginModule"/> performs
    /// its respective logout procedure which may include removing/destroying
    /// <see cref="AbstractSubject"/> informations and state cleanup.
    /// <para>
    /// Note that this method invokes all <see cref="ILoginModule"/> configured
    /// for the application regardless of their respective control flag.
    /// Essentially this means that requisite and sufficient semantics are
    /// ignored for this method. This guarantees that proper cleanup and state
    /// restoration can take place.
    /// </para>
    /// </remarks>
    public void Logout(ISubject subject) {
      foreach (ILoginModule login_module in login_modules_) {
        try {
          login_module.Logout(subject);
        } catch (Exception ex) {
          // Don't punish the other login modules if we're given a bad one.
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "Logout"), ex);
        }
      }
    }
  }
}
