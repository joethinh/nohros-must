using System;
using System.Web;
using Nohros.Security.Auth;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace Nohros.Security.Auth.ServiceStack
{
  /// <summary>
  /// A <see cref="Service"/> that can be used to handle authentication request
  /// that uses the well know "login/password" credentials.
  /// </summary>
  public abstract class AbstractAuthService : Service
  {
    readonly HttpAuthenticationManager authenticator_;

    protected AbstractAuthService(HttpAuthenticationManager authenticator) {
      authenticator_ = authenticator;
    }

    /// <summary>
    /// Attempts to sign the given subject in by using the given login and
    /// password information.
    /// </summary>
    /// <param name="subject">
    /// The subject to be signed in.
    /// </param>
    /// <param name="login">
    /// The login associated with the given subject.
    /// </param>
    /// <param name="password">
    /// The password of the given login.
    /// </param>
    /// <exception cref="HttpError.Unauthorized">
    /// The given login/password credentials are not valid.
    /// </exception>
    protected void SignIn(ISubject subject, string login, string password) {
      if (login == null || password == null) {
        OnUnauthorized();
      }

      var token = authenticator_.Authenticate(subject,
        new LoginPasswordCallbackHandler(login, password));
      if (!token.Authenticated) {
        OnUnauthorized();
      }
    }

    /// <summary>
    /// Sign the current signed in subject out.
    /// </summary>
    /// <remarks>
    /// If there is no subject signed in this method performs no operation.
    /// </remarks>
    public void SignOut() {
      authenticator_.SignOut(HttpContext.Current);
    }

    /// <summary>
    /// Method that is called when the authentication process fail.
    /// </summary>
    protected virtual void OnUnauthorized() {
      Response.AddHeader(HttpHeaders.WwwAuthenticate,
        "Auth br.com.nohros");
      throw HttpError.Unauthorized(Resources.Request_Unauthorized);
    }

    /// <summary>
    /// Gets a value indicating if the current request is associated with
    /// an authenticated subject.
    /// </summary>
    public bool IsAuthenticated {
      get {
        ISubject subject;
        return authenticator_.GetSubject(HttpContext.Current, out subject);
      }
    }
  }
}
