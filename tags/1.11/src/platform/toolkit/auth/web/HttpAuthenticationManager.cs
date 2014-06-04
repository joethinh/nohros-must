using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using Nohros.Caching;
using Nohros.Caching.Providers;
using Nohros.Extensions;

namespace Nohros.Security.Auth
{
  public class HttpAuthenticationManager : AuthenticationManager
  {
    public const string kTokenKey = "Nohros.Security.Auth.Token";
    public const string kCookieName = "NHSAUTHID";

    #region .ctor
    public HttpAuthenticationManager(LoginContext login_context,
      ICacheProvider cache) : base(login_context, cache) {
    }
    #endregion

    /// <summary>
    /// Authenticates the given subject using the associated
    /// <see cref="LoginContext"/> and if the login succeed, creates and
    /// attach a cookie in the response, containg the token associated with
    /// the authenticated subject.
    /// </summary>
    /// <param name="subject">
    /// The subject to be authenticated.
    /// </param>
    /// <param name="callback">
    /// The <see cref="IAuthCallbackHandler"/> that should be called to
    /// retrieve the <paramref name="subject"/> authentication information.
    /// </param>
    /// <remarks>
    /// The token of the authenticated subject is associated with the key
    /// <see cref="kTokenKey"/> on the <see cref="HttpContext.Items"/>
    /// collection of the <see cref="HttpContext.Current"/> object and the
    /// authenticated token is added to the associated
    /// <see cref="ICacheProvider"/> using the authentication token as a key.
    /// </remarks>
    public override AuthenticationToken Authenticate(ISubject subject,
      IAuthCallbackHandler callback) {
      AuthenticationToken token;
      Authenticate(subject, callback, HttpContext.Current, out token);
      return token;
    }

    /// <summary>
    /// Authenticates the given subject using the associated
    /// <see cref="LoginContext"/> and if the login succeed, creates and
    /// attach a cookie in the response, containg the token associated with
    /// the authenticated subject.
    /// </summary>
    /// <param name="subject">
    /// The subject to be authenticated.
    /// </param>
    /// <param name="callback">
    /// The <see cref="IAuthCallbackHandler"/> that should be called to
    /// retrieve the <paramref name="subject"/> authentication information.
    /// </param>
    /// <param name="context">
    /// The <see cref="HttpContext"/> object that should be used to store
    /// the authenticated subject.
    /// </param>
    /// <remarks>
    /// The token of the authenticated subject is associated with the key
    /// <see cref="kTokenKey"/> on the <see cref="HttpContext.Items"/>
    /// collection of the <paramref name="context"/> object and the
    /// authenticated token is added to the associated
    /// <see cref="ICacheProvider"/> using the authentication token as a key.
    /// </remarks>
    public bool Authenticate(ISubject subject, IAuthCallbackHandler callback,
      HttpContext context) {
      AuthenticationToken token;
      return Authenticate(subject, callback, context, out token);
    }

    bool Authenticate(ISubject subject, IAuthCallbackHandler callback,
      HttpContext context, out AuthenticationToken token) {
      if (context == null) {
        throw new ArgumentNullException("context");
      }

      token = base.Authenticate(subject, callback);
      if (!token.Authenticated) {
        return false;
      }

      var ticket = new FormsAuthenticationTicket(1, token.Token, DateTime.Now,
        DateTime.Now.AddMinutes(TokenExpiration.TotalMinutes), false,
        token.Token);

      string e_ticket = FormsAuthentication.Encrypt(ticket);

      var cookie = new HttpCookie(kCookieName,
        e_ticket.AsBase64(Encoding.Default));
      context.Items[kTokenKey] = token.Token;
      context.Response.SetCookie(cookie);
      return true;
    }

    /// <summary>
    /// Logs the use out and invalidates the authentication token.
    /// </summary>
    /// <returns>
    /// <c>true</c> if a logged <see cref="ISubject"/> is successfully logged
    /// out and its associated token invalidated.
    /// </returns>
    public void SignOut(HttpContext context) {
      if (context == null) {
        throw new ArgumentNullException("context");
      }

      var token = context.Items[kTokenKey] as string;
      if (token != null) {
        context.Items.Remove(kTokenKey);
        context.Response.Cookies.Remove(kCookieName);
        base.SignOut(token);
      }
    }

    /// <summary>
    /// Gets the <see cref="ISubject"/> that is associated with the current
    /// http request.
    /// </summary>
    /// <typeparam name="T">
    /// The type of subject to return.
    /// </typeparam>
    /// <param name="context">
    /// The login_context of the current HTTP request.
    /// </param>
    /// <param name="subject">
    /// When this method return, contains the subject that is associated with
    /// the current HTTP request.
    /// </param>
    /// <returns>
    /// <c>true</c> is a <see cref="ISubject"/> associated with the current
    /// HTTP request is found; otherwise, false.
    /// </returns>
    public bool GetSubject<T>(HttpContext context, out T subject)
      where T : ISubject {
      if (context == null) {
        throw new ArgumentNullException("context");
      }
      var token = context.Items[kTokenKey] as string;
      if (token == null) {
        subject = default(T);
        return false;
      }
      return GetSubject(token, out subject);
    }
  }
}
