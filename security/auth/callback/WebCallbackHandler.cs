using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// An implementation of the <see cref="IAuthCallbackHandler"/> interface that handles web callbacks such
  /// FieldCallback.
  /// </summary>
  /// <remarks>This class can handle the callbacks above:
  ///     FieldCallback
  /// </remarks>
  public class WebCallbackHandler: IAuthCallbackHandler
  {
    NameValueCollection _form;
    string _username;
    string _password;

    /// <summary>
    /// Default constructor
    /// </summary>
    public WebCallbackHandler() {
      HttpContext context = HttpContext.Current;
      _form = (context != null) ? context.Request.Form : new NameValueCollection();
      _username = null;
      _password = null;
    }

    /// <summary>
    /// Initializes a new instance_ of the WebAuthCallback by using the specified username and
    /// password information.
    /// </summary>
    /// <param name="username">The name of the user</param>
    /// <param name="password">The user password</param>
    /// <exception cref="ArgumentNullException">username or password is null</exception>
    public WebCallbackHandler(string username, string password) {
      if (username == null || password == null)
        throw new ArgumentNullException((username == null) ? "username" : "password");

      _username = username;
      _password = password;
    }

    public void Handle(IAuthCallback[] callbacks) {
      for (int i = 0; i < callbacks.Length; i++) {
        IAuthCallback c = callbacks[i];
        if (c is FieldCallback) {
          FieldCallback fc = c as FieldCallback;
          if (_username == null)
            fc.Value = _form[fc.Name];
          else {
            if (fc.Name == "username")
              fc.Value = _username;
            else if (fc.Name == "password")
              fc.Value = _password;
          }
        }
      }
    }
  }
}