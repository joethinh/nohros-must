using System;
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
    readonly LoginContext context_;

    #region .ctor
    public HttpAuthenticationManager(LoginContext context, ICacheProvider cache) {
      context_ = context;
      cache_ = cache;
    }
    #endregion

    public bool Authenticate(ISubject subject, IAuthCallbackHandler callback,
      HttpContext context) {
      if (!context_.Login(subject, callback)) {
        return false;
      }
      var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
      var ticket = new FormsAuthenticationTicket(1, token, DateTime.Now,
        DateTime.Now.AddMinutes(30), false, token);

      string e_ticket = FormsAuthentication.Encrypt(ticket);
      var cookie = new HttpCookie(kCookieName, e_ticket);
      context.Items[kTokenKey] = token;
      context.Response.Cookies.Add(cookie);
      return true;
    }

    public bool GetSubject<T>(HttpContext context, out T subject)
      where T : ISubject {
      object token = context.Items[kTokenKey] as string;
      if (token != null) {
        subject = cache_.Get<T>(token);
      }
    }
  }
}
