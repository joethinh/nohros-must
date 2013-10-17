using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Nohros.Caching;
using Nohros.Caching.Providers;

namespace Nohros.Security.Auth
{
  public class HttpAuthenticationManager
  {
    public const string kTokenKey = "Nohros.Security.Auth.Token";
    public const string kCookieName = "Nohros.Security.Auth.Cookie";

    readonly ICacheProvider cache_;
    readonly LoginContext login_context_;

    #region .ctor
    public HttpAuthenticationManager(LoginContext login_context,
      ICacheProvider cache) {
      if (login_context == null || cache == null) {
        throw new ArgumentNullException(login_context == null
          ? "login_context"
          : "cache");
      }
      login_context_ = login_context;
      cache_ = cache;
    }
    #endregion

    public bool Authenticate(ISubject subject, IAuthCallbackHandler callback,
      HttpContext context) {
      if (subject == null || callback == null || context == null) {
        throw new ArgumentNullException(context == null
          ? "context"
          : callback == null
            ? "callback"
            : "context");
      }

      if (!login_context_.Login(subject, callback)) {
        return false;
      }

      var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

      cache_.Add(token, subject);

      var ticket = new FormsAuthenticationTicket(1, token, DateTime.Now,
        DateTime.Now.AddMinutes(ExpirationInMinutes), false, token);
      string e_ticket = FormsAuthentication.Encrypt(ticket);

      var cookie = new HttpCookie(kCookieName, e_ticket);
      context.Items[kTokenKey] = token;
      context.Response.Cookies.Add(cookie);
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
        cache_.Remove(token);
        context.Items.Remove(kTokenKey);
        context.Response.Cookies.Remove(kCookieName);
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
      return cache_.Get(token, out subject);
    }

    /// <summary>
    /// Gets or set a value indicating
    /// </summary>
    public int ExpirationInMinutes { get; set; }
  }
}
