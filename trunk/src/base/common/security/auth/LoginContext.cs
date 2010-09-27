using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Principal;

using Nohros.Resources;

namespace Nohros.Security.Auth
{
    /// <summary>
    /// Describes the basic methods used to authenticate Users and provides a
    /// way to develop an application independent of the underlying authentication
    /// technology. A configuration specifies the <see cref="LoginModule"/>, to be
    /// used with a particular application. 
    /// <para>
    /// In adition to supporting pluggable authentication, this class also support
    /// the notion of stacked authentication. Applications may be configured to use
    /// more than on <see cref="LoginModule"/>
    /// </para>
    /// </summary>
    public sealed class LoginContext
    {
        IAuthCallbackHandler callback_;
        Subject subject_;
        ILoginConfiguration config_;
        bool provided_subject_;
        Dictionary<string, object> state_;

        #region .ctor
        /// <summary>
        /// Initialize a new instance_ of the LoginContext.
        /// </summary>
        public LoginContext()
        {
            provided_subject_ = false;
            subject_ = new Subject();
            state_ = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance_ of the <see cref="LoginContext"/> class by
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
        /// Logs the <see cref="Principal"/> out, cleaning up any state that may be in memory.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method invokes the logout method for each <see cref="LoginModule"/> configured for
        /// this <see cref="LoginContext"/>. Each <see cref="LoginModule"/> performs its respective
        /// logout procedure whicj may include removing/destroying <see cref="Principal"/> informations and
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