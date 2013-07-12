using System;
using System.Collections.Generic;
using Nohros.Configuration;
using Nohros.Logging;
using Nohros.Resources;

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
  /// permissions with the <see cref="Subject"/>. Each login module's abort
  /// method cleans up or removes/destroys any previously stored authentication
  /// state.
  /// </para>
  /// <para>
  /// If the login method returns without throwing an exception, then the
  /// overall authentication succeeded. The caller can then retrieve the
  /// newly authenticated <see cref="Subject"/> by getting the value of the
  /// <see cref="Subject"/> property. Permissions associated with the subject
  /// may be retrieved by getting the value associated with the subject
  /// respective <see cref="Subject.Permissions"/> property.
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
  /// initialized with a <see cref="Subject"/> object to be authenticated, a
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
  /// <seealso cref="Subject"/>
  /// <seealso cref="IAuthCallbackHandler"/>
  /// <seealso cref="ILoginModule"/>
  public sealed class LoginContext
  {
    readonly ILoginModule[] login_modules_;
    readonly bool subject_was_suplied_;
    Subject subject_;

    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="LoginContext"/> class by
    /// using the specified login modules.
    /// </summary>
    /// <param name="login_modules">
    /// A array containing all the configuration data and its associated
    /// login modules that should be used to authenticate a subject using this
    /// context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="login_modules"/> is <c>null</c>.
    /// </exception>
    public LoginContext(ILoginModule[] login_modules) {
      if (login_modules == null) {
        throw new ArgumentNullException("login_modules");
      }
      login_modules_ = login_modules;
      subject_was_suplied_ = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginContext"/> class by
    /// using the specified subject and login modules.
    /// </summary>
    /// <param name="subject">
    /// The <see cref="Subject"/> to authenticate.
    /// </param>
    /// <param name="login_modules">
    /// A array containing all the configuration data and its associated
    /// login modules that should be used to authenticate a subject using this
    /// context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="subject"/> or <paramref name="login_modules"/>
    /// are <c>null</c>.
    /// </exception>
    public LoginContext(Subject subject, ILoginModule[] login_modules)
      : this(login_modules) {
      if (subject == null || login_modules == null) {
        throw new ArgumentNullException(subject == null
          ? "subject"
          : "login_modules");
      }
      subject_ = subject;
      subject_was_suplied_ = true;
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
    /// <see cref="IPermission"/> with the <see cref="Subject"/>.
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
    public bool Login() {
      int i, j;
      bool overall_login_succeeds = true;
      IList<ILoginModule> succeeded_login_modules =
        new List<ILoginModule>(login_modules_.Length);

      for (i = 0, j = login_modules_.Length; i < j; i++) {
        ILoginModule login_module = login_modules_[i];

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
        Abort();
        return false;
      }
      return Commit(succeeded_login_modules);
    }

    /// <summary>
    /// Execute the commit method for the login modules that has succeed.
    /// </summary>
    /// <param name="succeeded_login_modules"></param>
    /// <returns></returns>
    bool Commit(IList<ILoginModule> succeeded_login_modules) {
      try {
        for (int i = 0, j = succeeded_login_modules.Count; i < j; i++) {
          ILoginModule login_module = succeeded_login_modules[i];
          login_module.Commit();
        }
      } catch (Exception exception) {
        MustLogger.ForCurrentProcess.Debug(string.Format(
          StringResources.Log_ThrowsException, "commit"), exception);

        Abort();
        return false;
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
    void Abort() {
      for (int i = 0, j = login_modules_.Length; i < j; i++) {
        ILoginModule login_module = login_modules_[i];
        try {
          login_module.Abort();
        } catch (Exception exception) {
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "commit"), exception);
        }
      }
      if (!subject_was_suplied_) {
        subject_ = null;
      }
    }

    /// <summary>
    /// Logs the <see cref="Subject"/> out, cleaning up any state that may be
    /// in memory.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This method invokes the logout method for each
    /// <see cref="ILoginModule"/> configured for this
    /// <see cref="LoginContext"/>. Each <see cref="ILoginModule"/> performs
    /// its respective logout procedure which may include removing/destroying
    /// <see cref="Subject"/> informations and state cleanup.
    /// <para>
    /// Note that this method invokes all <see cref="ILoginModule"/> configured
    /// for the application regardless of their respective control flag.
    /// Essentially this means that requisite and sufficient semantics are
    /// ignored for this method. This guarantees that proper cleanup and state
    /// restoration can take place.
    /// </para>
    /// </remarks>
    public void Logout() {
      for (int i = 0, j = login_modules_.Length; i < j; i++) {
        ILoginModule login_module = login_modules_[i];
        try {
          login_module.Logout();
        } catch (Exception ex) {
          // Don't punish the other login modules if we're given a bad one.
          MustLogger.ForCurrentProcess.Error(string.Format(
            StringResources.Log_MethodThrowsException, "logout"), ex);
        }
      }
    }

    /// <summary>
    /// Gets the authenticated <see cref="Subject"/>.
    /// </summary>
    /// <remarks>
    /// If the caller was specified a <see cref="Subject"/>, this method
    /// returns the caller-specified <see cref="Subject"/>. If a
    /// <see cref="Subject"/> was not specified and authentication succeeds,
    /// this method returns the <see cref="Subject"/> instantiated and used for
    /// authentication by this <see cref="LoginContext"/>. If a
    /// <see cref="Subject"/> was not specified, and authentication fails or
    /// has been attempted, this method returns null.
    /// </remarks>
    public Subject Subject {
      get { return subject_; }
    }
  }
}
