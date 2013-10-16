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
    readonly
      IEnumerable<LoginModuleFactoryTuple> login_module_factories_;

    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="LoginContext"/> class by
    /// using the specified login modules.
    /// </summary>
    /// <param name="login_module_factories">
    /// A array containing the factories that can be used to create an instance
    /// of all the configured <see cref="ILoginModule"/>s.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="login_module_factories"/> is <c>null</c>.
    /// </exception>
    public LoginContext(
      IEnumerable<LoginModuleFactoryTuple> login_module_factories) {
      if (login_module_factories == null) {
        throw new ArgumentNullException("login_module_factories");
      }
      login_module_factories_ = login_module_factories;
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
      int i, j;
      bool overall_login_succeeds = true;

      ILoginModule[] login_modules = CreateLoginModules(subject,
        auth_callback_handler);
      IList<ILoginModule> succeeded_login_modules =
        new List<ILoginModule>(login_modules.Length);

      for (i = 0, j = login_modules.Length; i < j; i++) {
        ILoginModule login_module = login_modules[i];

        // A try/catch block is used here to ensure that the login method will
        // be called for each configured module (respecting the control flag
        // semantics).
        bool login_succeeds;
        try {
          login_succeeds = login_module.Login();
        } catch (Exception ex) {
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "login"), ex);
          login_succeeds = false;
        }

        if (login_succeeds) {
          succeeded_login_modules.Add(login_module);
          if (login_module.ControlFlag == LoginModuleControlFlag.Sufficient) {
            break;
          }
        } else {
          // The login has failed, if the failed module is "requisite" or
          // "required" the overall login should fail.
          LoginModuleControlFlag login_module_control_flag =
            login_module.ControlFlag;

          // The login has failed, if the failed module is "requisite" or
          // "required" the overall login should fail.
          if (login_module_control_flag == LoginModuleControlFlag.Requisite) {
            // if the failed module is "requisite" we need to stop procceding
            // down the login module list.
            overall_login_succeeds = false;
            break;
          }

          if (login_module_control_flag == LoginModuleControlFlag.Required) {
            overall_login_succeeds = false;
          }
        }
      }

      if (!overall_login_succeeds) {
        Abort(login_modules);
        return false;
      }

      try {
        if (!Commit(succeeded_login_modules)) {
          Abort(login_modules);
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
    /// <param name="succeeded_login_modules"></param>
    /// <returns></returns>
    bool Commit(IList<ILoginModule> succeeded_login_modules) {
      for (int i = 0, j = succeeded_login_modules.Count; i < j; i++) {
        ILoginModule login_module = succeeded_login_modules[i];
        if (!login_module.Commit()) {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Call the <see cref="ILoginModule.Abort"/> method for all configured
    /// login modules.
    /// </summary>
    /// <remarks>
    /// If a <see cref="ILoginModule"/> throws an exception, it will be logged
    /// and we proceed down the list of login modules.
    /// </remarks>
    void Abort(ILoginModule[] login_modules) {
      for (int i = 0, j = login_modules.Length; i < j; i++) {
        ILoginModule login_module = login_modules[i];
        try {
          login_module.Abort();
        } catch (Exception exception) {
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "commit"), exception);
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
    public void Logout(AbstractSubject subject) {
      ILoginModule[] login_modules = CreateLoginModules(subject,
        new NopAuthCallbackHandler());
      for (int i = 0, j = login_modules.Length; i < j; i++) {
        ILoginModule login_module = login_modules[i];
        try {
          login_module.Logout(subject);
        } catch (Exception ex) {
          // Don't punish the other login modules if we're given a bad one.
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "logout"), ex);
        }
      }
    }

    ILoginModule[] CreateLoginModules(ISubject subject,
      IAuthCallbackHandler auth_callback_handler) {
      var shared_state = new Dictionary<string, string>();
      return login_module_factories_
        .Select(
          tuple =>
            tuple.Key.CreateLoginModule(subject, auth_callback_handler,
              shared_state, tuple.Value))
        .ToArray();
    }
  }
}
