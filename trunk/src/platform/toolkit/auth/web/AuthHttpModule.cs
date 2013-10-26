using System;
using System.Web;
using System.Web.Security;

namespace Nohros.Security.Auth
{
  public class AuthHttpModule : IHttpModule
  {
    const string kTokenKey = HttpAuthenticationManager.kTokenKey;
    const string kCookieName = HttpAuthenticationManager.kCookieName;

    public void Init(HttpApplication app) {
      FormsAuthentication.Initialize();
      app.AuthenticateRequest += OnAuthenticate;
    }

    public void Dispose() {
    }

    void OnAuthenticate(object source, EventArgs e) {
      var app = (HttpApplication) source;
      HttpContext context = app.Context;

      Listeners.SafeInvoke<AuthenticationEventHandler>(Authenticate,
        handler => handler());

      FormsAuthenticationTicket ticket;
      // If the request does not have an authentication ticket associated
      // the user is not authenticated.
      if (!GetTicketFromCookie(context, kCookieName,
        out ticket)) {
        return;
      }

      // If the ticket is expired the user should login again.
      if (!ticket.Expired) {
        return;
      }

      // The user data contains the token of the logged in user.
      context.Items[kTokenKey] = ticket.UserData;
    }

    public bool GetTicketFromCookie(HttpContext context, string name,
      out FormsAuthenticationTicket ticket) {
      ticket = null;
      HttpCookie cookie = context.Request.Cookies[name];
      if (cookie == null) {
        return false;
      }

      try {
        ticket = FormsAuthentication.Decrypt(cookie.Value);
        return true;
      } catch {
        context.Request.Cookies.Remove(name);
      }
      return false;
    }

    public event AuthenticationEventHandler Authenticate;
  }
}
