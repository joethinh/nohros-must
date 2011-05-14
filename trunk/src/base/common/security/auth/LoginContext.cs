using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Principal;

using Nohros.Resources;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Describes the basic methods used to authenticate <see cref="Subject"/>s and provides a way to
    /// develop an application independent of the underlying authentication technology.
    /// </summary>
    /// <remarks> A <see cref="LoginConfiguration"/> specifies the authentication technology, or
    /// <see cref="LoginModule"/>, to be used with a particular application. Therefore, different login
    /// modules can be plugged in under an application without requiring any modifications to the
    /// application itself.
    /// <para>
    /// In adition to supporting pluggable authentication, this class also support the notion of stacked
    /// authentication. In other words, an application may be configured to use more than one
    /// <see cref="LoginModule"/>. For example, one could configure both Kerberos login module and a
    /// smart card login module under an application
    /// </para>
    /// <para>
    /// A typical caller instantiates this class and passes in a name and a <see cref="IAuthCallbackHandler"/>.
    /// <see cref="LoginContext"/> uses the name as the index into the <see cref="LoginConfiguration"/>
    /// to determine which login module should be used, and which ones must succeed in order  for the
    /// overall authentication to succeed. The <see cref="iAuthCallbackHandler"/> object is passed to the
    /// underlying login modules so they may communicate and interact with users(prompting for a username
    /// and password via a graphical user interface, for example.
    /// </para>
    /// <para>
    /// Once the caller has instantiated a <see cref="LoginContext"/>, it invokes the
    /// <see cref="LoginModule.Login()"/> method for each login module configured for the name specified
    /// by the caller. Each login module then performs its respective type of authentication(username/
    /// password, smart card pin verification, etc). Note that the login module will not attempt
    /// authentication retries or introduces delays if the authentication fails. Such tasks belong to the
    /// caller.
    /// </para>
    /// <para>
    /// Regardless of whether or not the overall authentication secceeded, this login method completes a
    /// 2-phase authentication process by then calling either the <see cref="LoginModule.Commit()"/>
    /// method or the <see cref="LoginModule.Abort()"/> method for each of the configured login modules.
    /// The commit method for each login module gets invoked if the overall authentication secceeded,
    /// whereas the abort method for each login module gets invoked if the overall authentication failed.
    /// Each successful login module's commit method associates the relevant permissions with the
    /// <see cref="Subject"/>. Each login module's abort method cleans up or removes/destroys any
    /// previously stored authentication state.
    /// </para>
    /// <para>
    /// If the login method returns without throwing an exception, then the overall authentication
    /// succeeded. The caller can then retrieve the newly authenticated <see cref="Subject"/> by getting
    /// the value of the <see cref="Subject"/> property. Permissions associated with the subject may be
    /// retrieved by getting the value associated with the subject respective
    /// <see cref="Subject.Permissions"/> property.
    /// </para>
    /// <para>
    /// To logout the subject, the caller simple needs to invoke the <see cref="Logout"/> method. As with
    /// the <see cref="Login"/> method, this logout method invokes the <see cref="LoginModule.Logout"/>
    /// method for each login module configured for this login context. Each login module's logout method
    /// cleans up state and removes/destroys permissions from the subject as appropriate.
    /// </para>
    /// <para>
    /// Each of the configured login module invoked by the login context is initialized with a
    /// <see cref="Subject"/> object to be authenticated, a <see cref="ICallbackHandler"/> object used to
    /// communicate with users, shared login module state, and login module specific options.
    /// </para>
    /// <para>
    /// Each login module which successfully authenticates a user updates the subject with the relevant
    /// user information(permissions). This subject can then be returned via the subject method from the
    /// <see cref="LoginContext"/> class if the overall authentication succeeds.
    /// </para>
    /// <para>
    /// A login context supports authentication retries by calling application. For example, a login
    /// context's login method may be invoked mutiple times if the user incorrectly types in a password.
    /// However, a login context should not be used to authenticate more than one subject. A separate
    /// login context should be used to authenticate each different subject.
    /// </para>
    /// <para>
    /// Mutiple calls into the same login context do not affect the login module state, or the login
    /// module specific options.
    /// </para>
    /// </remarks>
    /// <seealso cref="Subject"/>
    /// <seealso cref="IAuthCallbackHandler"/>
    /// <seealso cref="ILoginConfiguration"/>
    /// <seealso cref="ILoginModule"/>
    public sealed class LoginContext
    {
        IAuthCallbackHandler callback_;
        Subject subject_;
        ILoginConfiguration config_;
        bool provided_subject_;
        Dictionary<string, object> state_;

        #region .ctor
        /// <summary>
        /// Initialize a new instance of the <see cref="LoginContext"/> class.
        /// </summary>
        public LoginContext()
        {
            provided_subject_ = false;
            subject_ = new Subject();
            state_ = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginContext"/> class by
        /// using the specified subject object.
        /// </summary>
        /// <param name="subject">The <see cref="Subject"/> to authenticate. The
        /// value can be null.</param>
        /// <remarks>If the caller specifies a null <see cref="Subject"/>, the LoginContext
        /// instantiates a new <see cref="Subject"/>; otherwise the caller-specified <see cref="Subject"/>
        /// object will be used</remarks>
        public LoginContext(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");
            subject_ = subject;
            provided_subject_ = true;
            state_ = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance_ of the <see cref="LoginContext"/> class by
        /// using the specified <see cref="IAuthCallbackHandler"/> object.
        /// </summary>
        /// <param name="callback">The <see cref="IAuthCallbackHandler"/> object used by
        /// <see cref="LoginModules"/>(s) to communicate with the user. The value
        /// can be null.</param>
        /// <remarks>If the caller specifies a null <see cref="Subject"/>, the LoginContext
        /// instantiates a new <see cref="Subject"/>; otherwise the caller-specified <see cref="Subject"/>
        /// object will be used</remarks>
        public LoginContext(IAuthCallbackHandler callback):this()
        {
            if (callback == null)
                throw new ArgumentNullException("callback");
            callback_ = callback;
        }

        /// <summary>
        /// Initializes a new instance_ of the LoginContext by using the specified configuration object.
        /// </summary>
        /// <param name="config">The <see cref="ILoginConfiguration"/> object that lists the login modules
        /// to perform the authentication.</param>
        public LoginContext(ILoginConfiguration config):this() {
            if (config == null)
                throw new ArgumentNullException("config");
            config_ = config;
        }

        /// <summary>
        /// Initializes a new instance_ of the LoginContext class by using the
        /// specified <see cref="Subject"/> object and <see cref="IAuthCallbackHandler"/> delegate.
        /// </summary>
        /// <param name="subject">The subject to authenticate</param>
        /// <param name="callback">The <see cref="IAuthCallbackHandler"/> object used by
        /// <see cref="LoginModule"/>(s) to communicate with the user. The value can be null.</param>
        /// <remarks>If the caller specifies a null <see cref="Subject"/>, the LoginContext
        /// instantiates a new <see cref="Subject"/>; otherwise the caller-specified <see cref="Subject"/>
        /// object will be used</remarks>
        public LoginContext(Subject subject, IAuthCallbackHandler callback)
        {
            if (subject == null || callback == null)
                throw new ArgumentNullException((subject == null) ? "subject" : "callback");
            subject_ = subject;
            state_ = new Dictionary<string, object>();
            provided_subject_ = true;
        }

        /// <summary>
        /// Initializes a new instance_ of the <see cref="LoginContext"/> class with
        /// a <see cref="Subject"/> object, a <see cref="IAuthCallbackHandler"/> object, and a
        /// <see cref="ILoginConfiguration"/> object.
        /// </summary>
        /// <param name="subject">The subject to authenticate, or null.</param>
        /// <param name="callback">The <see cref="AuthCallbakc"/> object used by
        /// <see cref="LoginModule"/>(s) to communicate with the user, or null.</param>
        /// <param name="config">The <see cref="ILoginConfiguration"/> object that lists the login modules
        /// to perform the authentication, or null.</param>
        public LoginContext(Subject subject, IAuthCallbackHandler callback, ILoginConfiguration config)
        {
            config_ = config;
            callback_ = callback;
            provided_subject_ = true;
            subject_ = subject;
            state_ = new Dictionary<string, object>();

            // initialize the modules
            ILoginModuleEntry[] entries = config_.LoginModules;
            if (entries != null) {
                for (int i = 0, j = entries.Length; i < j; i++) {
                    ILoginModuleEntry entry = entries[i];
                    if (entry.Module == null)
                        throw new LoginException(StringResources.GetString(StringResources.Auth_LoginMudule_Type, entry.Type.Name));
                    entry.Module.Init(subject_, callback, state_, entry.Options);
                }
            }
        }
        #endregion

        /// <summary>
        /// Performs the authentication.
        /// </summary>
        /// <remarks>
        /// This method invokes the login method for each configured <see cref="LoginModule"/>, as
        /// determined by the current <see cref="ILoginConfiguration"/> instance_. Each <see cref="LoginModule"/>
        /// then performs its respective type of authentication(username/password, smart card pin verification, etc.)
        /// <para>
        /// The method completes a 2-phase authentication process by calling each configured <see cref="LoginModule.Commit()"/>
        /// method if the overall authentication succeeded(the relevant REQUIRED, REQUISITE, SUFFICIENT, and OPTIONAL
        /// <see cref="LoginModule"/>(s) succeeded, or by calling each configured <see cref="LoginModule.Abort"/>
        /// method if the overall authentication failed. If the authentication succeeded, each successful
        /// <see cref="LoginModule.Commit"/> method associates the relevant <see cref="IRoles"/> with the <see cref="Subject"/>.
        /// If authentication failed, each <see cref="LoginModule.Abort"/> method removes/destroys any previous stored
        /// state.
        /// <para>
        /// If the commit phase of the authentication process fails, then the overall authentication fails and
        /// this method invokes the abort method for each configured <see cref="LoginModule"/>.
        /// <para>
        /// If the abort method fails for any reason, the overall authentication fails.
        /// </para>
        /// </para>
        /// Note that if this method enters the abort phase(either the login or commit phase failed), this method
        /// invokes all <see cref="LoginModule"/> configured for the application regardless of their respective
        /// configuration flag parameters. Essentially this means that Required and Sufficient semantics are ignored
        /// during the abort phase. This guarantees that proper cleanup and state restoration can take place.
        /// </para>
        /// </remarks>
        public bool Login()
        {
            int i = 0, j;
            bool failure = false;

            ILoginModuleEntry[] entries = config_.LoginModules;

            // If there is no module nothing we can do.
            if (entries == null || (j = entries.Length) == 0)
                return false;

            try
            {
                for (i = 0, j = entries.Length; i < j; i++)
                {
                    ILoginModuleEntry entry = entries[i];

                    bool result = entry.Module.Login();
                    if (!result)
                    {
                        if (entry.ControlFlag == LoginModuleControlFlag.REQUISITE ||
                            entry.ControlFlag == LoginModuleControlFlag.REQUIRED)
                        {
                            failure = true;
                            break;
                        }
                    }
                    else
                    {
                        if (entry.ControlFlag == LoginModuleControlFlag.SUFFICIENT)
                            break;
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                failure = true;
            }
#else
            catch
            {
                failure = true;
            }
#endif

LBL_ABORT:
            if (failure)
            {
                // abort the login phase
                while (i >= 0)
                {
                    // we cannot avoid a try catch inside this loop.
                    // There must be a call to abort for each successful call to login
                    try { entries[i--].Module.Abort(); }
                    catch { }
                }

                if(!provided_subject_)
                    subject_ = null;
                return false;
            }

            j = i;
            // commit phase
            while (i >= 0)
            {
                // we cannot avoid a try catch inside this loop.
                // There must be a call to commit for each successful call to login
                try {
                    if (failure = !entries[i--].Module.Commit())
                        break;
                }
                catch {
                    failure = false;
                    break;
                }
            }

            // If the abort phase fails, then the overall authentication fails
            if (failure)
            {
                i = j;
                goto LBL_ABORT;
            }

            return true;
        }

        /// <summary>
        /// Logs the <see cref="Subject"/> out, cleaning up any state that may be in memory.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method invokes the logout method for each <see cref="LoginModule"/> configured for
        /// this <see cref="LoginContext"/>. Each <see cref="LoginModule"/> performs its respective
        /// logout procedure which may include removing/destroying <see cref="Subject"/> informations and
        /// state cleanup.
        /// <para>
        /// Note that this method invokes all <see cref="LoginModule"/> configured for the application
        /// regardless of their respective control flag. Essentially this means that REQUISITE and SUFFICIENT
        /// semantics are ignored for this method. This guarantees that proper cleanup and state restoration
        /// can take place.
        /// </para>
        /// </remarks>
        public void Logout()
        {
            ILoginModuleEntry[] entries = config_.LoginModules;
            for (int i = 0, j = entries.Length; i < j; i++) {
                // we cannot avoid a try catch inside this loop.
                // we need to call the logout for each module.
                try { entries[i].Module.Logout(); }
                catch { };
            }
        }

        /// <summary>
        /// Gets the authenticated <see cref="Subject"/>.
        /// </summary>
        /// <remarks>
        /// If the caller specified a <see cref="Subject"/> to this <see cref="LoginContext"/> constructor,
        /// this method returns the caller-specified <see cref="Subject"/>. If a <see cref="Subject"/> was
        /// not specified and authentication succeeds, this method returns the <see cref="Subject"/> instantiated
        /// and used for authentication by this <see cref="LoginContext"/>. If a <see cref="Subject"/> was not
        /// specified, and authentication fails or has been attempted, this method returns null.
        /// </remarks>
        public Subject Subject
        {
            get { return subject_; }
        }
    }
}