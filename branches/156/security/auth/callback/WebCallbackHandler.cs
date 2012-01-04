using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// An implementation of the <see cref="IAuthCallbackHandler"/> interface
  /// that handles web callbacks such <see cref="FieldCallback"/>.
  /// </summary>
  /// <remarks>This class can handle the callbacks above:
  /// <para>
  /// <see cref="FieldCallback"/>
  /// </para>
  /// </remarks>
  public class WebCallbackHandler: IAuthCallbackHandler
  {
    NameValueCollection form_;
    string username_;
    string password_;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebCallbackHandler"/>
    /// class.
    /// </summary>
    public WebCallbackHandler() {
      HttpContext context = HttpContext.Current;
      form_ = (context != null) ?
        context.Request.Form : new NameValueCollection();
      username_ = string.Empty;
      password_ = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebAuthCallback"/> by
    /// using the specified username and password information.
    /// </summary>
    /// <param name="username">The name of the user</param>
    /// <param name="password">The user password</param>
    /// <exception cref="ArgumentNullException">username or password is null</exception>
    public WebCallbackHandler(string username, string password) {
      if (username == null || password == null)
        throw new ArgumentNullException((username == null) ? "username" : "password");

      username_ = username;
      password_ = password;
    }

    public void Handle(IAuthCallback[] callbacks) {
      for (int i = 0; i < callbacks.Length; i++) {
        IAuthCallback callback = callbacks[i];
        if (callback is FieldCallback) {
          FieldCallback field_callback = callback as FieldCallback;
          if (username_ == null)
            field_callback.Value = form_[field_callback.Name];
          else {
            if (field_callback.Name == "username")
              field_callback.Value = username_;
            else if (field_callback.Name == "password")
              field_callback.Value = password_;
          }
        }
      }
    }
  }
}