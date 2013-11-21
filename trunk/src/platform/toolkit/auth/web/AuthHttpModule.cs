using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Security;
using Nohros.Extensions;

namespace Nohros.Security.Auth
{
  public class AuthHttpModule : IHttpModule
  {
    const string kTokenKey = HttpAuthenticationManager.kTokenKey;
    const string kCookieName = HttpAuthenticationManager.kCookieName;

    /// <summary>
    /// The key that should be added ot the context.Items to debug this
    /// module.
    /// </summary>
    public const string kDebugKey = "Nohros.Security.Auth.Debug";

    public void Init(HttpApplication app) {
      FormsAuthentication.Initialize();
      app.AuthenticateRequest += OnAuthenticate;
    }

    public void Dispose() {
    }

    void OnAuthenticate(object source, EventArgs e) {
      var app = (HttpApplication) source;
      HttpContext context = app.Context;

      if (context.Items.Contains(kDebugKey)) {
        Debugger.Break();
      }

      FormsAuthenticationTicket ticket;
      // If the request does not have an authentication ticket associated
      // the user is not authenticated.
      if (!GetTicketFromCookie(context, kCookieName,
        out ticket)) {
        return;
      }

      // If the ticket is expired the user should login again.
      if (ticket.Expired) {
        return;
      }

      // The user data contains the token of the logged in user.
      context.Items[kTokenKey] = ticket.UserData;
    }

    bool GetTicketFromCookie(HttpContext context, string name,
      out FormsAuthenticationTicket ticket) {
      ticket = null;
      HttpCookie cookie = context.Request.Cookies[name];
      if (cookie == null) {
        return false;
      }

      try {
        string cookie_data = cookie.Value.FromBase64String(Encoding.Default);
        ticket = FormsAuthentication.Decrypt(cookie_data);
        return true;
      } catch {
        context.Request.Cookies.Remove(name);
      }
      return false;
    }
  }
}
