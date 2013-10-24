using System;
using System.Web;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace Nohros.Security.Auth.ServiceStack
{
  [Secure]
  public class SecureService : Service
  {
    readonly HttpAuthenticationManager authenticator_manager_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SecureService"/> class
    /// by using the given <see cref="HttpAuthenticationManager"/> object.
    /// </summary>
    /// <param name="authenticator_manager">
    /// A <see cref="HttpAuthenticationManager"/> object that can be used to
    /// get a instance of the currently logged in <see cref="ISubject"/>.
    /// </param>
    public SecureService(HttpAuthenticationManager authenticator_manager) {
      authenticator_manager_ = authenticator_manager;
    }
    #endregion

    /// <summary>
    /// Gets the <see cref="ISubject"/> object that is associated with the
    /// current HTTP request.
    /// </summary>
    /// <exception cref="HttpError.Unauthorized">
    /// There is not subject associated with the current request.
    /// </exception>
    public ISubject Subject {
      get {
        ISubject subject = null;
        var context = HttpContext.Current;
        // Sanity check the context and subject.
        if (authenticator_manager_.GetSubject(context, out subject)) {
          throw HttpError.Unauthorized(Resources.Request_Unauthorized);
        }
        return subject;
      }
    }
  }
}
