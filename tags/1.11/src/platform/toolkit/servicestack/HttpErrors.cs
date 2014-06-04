using System;
using System.Net;
using ServiceStack.Common.Web;
using Nohros.ServiceStack;

namespace Nohros.Security.Auth.ServiceStack
{
  /// <summary>
  /// A useful class that contains shortcuts for the construction of HttpError
  /// objects.
  /// </summary>
  public static class HttpErrors
  {
    /// <summary>
    /// Constructs a new <see cref="HttpError"/> that is related with the
    /// <see cref="HttpStatusCode.InternalServerError"/> error code and
    /// contains the default error message.
    /// </summary>
    public static Exception ServerError() {
      return ServerError(Resources.Request_InternalServerError);
    }

    /// <summary>
    /// Constructs a new <see cref="HttpError"/> that is related with the
    /// <see cref="HttpStatusCode.InternalServerError"/> error code and
    /// contains the given <paramref name="message"/>.
    /// </summary>
    public static Exception ServerError(string message) {
      return new HttpError(HttpStatusCode.InternalServerError, message);
    }
  }
}
