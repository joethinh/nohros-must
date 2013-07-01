using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// Provides a skeletal implemetation of the <see cref="ILoginModule"/>
  /// interface to reduce the effort required to implement this interface.
  /// </summary>
  public abstract class AbstractLoginModule : ILoginModule
  {
    /// <summary>
    /// A <see cref="IAuthCallbackHandler"/> that can be used to communicate
    /// with the subject.
    /// </summary>
    protected IAuthCallbackHandler auth_callback_handler;

    /// <summary>
    /// The subject to be authenticated.
    /// </summary>
    protected Subject subject;

    /// <summary>
    /// A <see cref="IDictionary{TKey,TValue}"/> that contains the state that
    /// is shared between login modules.
    /// </summary>
    protected IDictionary<string, object> shared_state;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractLoginModule"/>
    /// class.
    /// </summary>
    protected AbstractLoginModule() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractLoginModule"/>
    /// class by using the specified subject and callback handler and shared
    /// state.
    /// </summary>
    protected AbstractLoginModule(Subject subject,
      IAuthCallbackHandler auth_callback_handler,
      IDictionary<string, object> shared_state) {
      this.subject = subject;
      this.auth_callback_handler = auth_callback_handler;
      this.shared_state = shared_state;
    }
    #endregion

    /// <inheritdoc/>
    public abstract bool Abort();

    /// <inheritdoc/>
    public abstract bool Commit();

    /// <inheritdoc/>
    public abstract bool Login();

    /// <inheritdoc/>
    public abstract bool Logout();
  }
}
