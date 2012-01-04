using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Nohros.Security.Auth
{
  /// <summary>
  /// The SecurityContext class is used for access control operations and decisions. Usually, this class
  /// is used to decide whether an access to a protected system resource is to be allowed or denied.
  /// </summary>
  public class SecurityContext: SecurityContextBase
  {
    const string cacheKey = "NasSecurityContext";

    /// <summary>
    /// Gets the <see cref="SecurityContext"/> object for the current request.
    /// </summary>
    public static SecurityContext Current {
      get {
        SecurityContext sContext = null;
        HttpContext context = HttpContext.Current;
        if (context != null) {
          sContext = context.Items[cacheKey] as SecurityContext;
          if (sContext == null)
            sContext = new SecurityContext();
          context.Items[cacheKey] = sContext;
          return sContext;
        }

        return new SecurityContext();
      }
    }

    #region .ctor
    private SecurityContext() {
    }

    /// <summary>
    /// Initializes a new instance_ of the SecurityContext class by using the specified
    /// <see cref="Subject"/>
    /// </summary>
    /// <param name="subject">The current subject</param>
    public SecurityContext(Subject subject) {
      if (subject == null)
        throw new ArgumentNullException("subject");

      _subject = subject;
    }
    #endregion
  }
}