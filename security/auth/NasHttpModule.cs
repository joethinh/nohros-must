using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Nohros.Security.Auth
{
    public class NasHttpModule : IHttpModule
    {
        /// <summary>
        /// Initializes this moduleand prepares it to handle request.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(nas_AuthenticateRequest);
        }

        public void nas_AuthenticateRequest(object sender, EventArgs e)
        {
            Subject subject = SecurityContext.Current.Subject;
            if (subject == null)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

                if(authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                {
                    // There is no authentication cookie, so the user is not authenticated.
                    return;
                }

                FormsAuthenticationTicket ticket = null;
                try {
                    ticket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch {
                    // cookie failed to decrypt
                    return;
                }

                subject = Subject.Get(ticket);

                // If the subject is null the ticket has been expired.
                if (subject == null)
                    return;

                SecurityContext.Current.Subject = subject;
            }
        }

        public void Dispose()
        {
        }
    }
}