using System;
using System.Web;
using ZMQ;

namespace Nohros.Toolkit.RestQL
{
  public class HttpQueryApp
  {
    #region .ctor
    public HttpQueryApp() {
    }
    #endregion

    Configure()

    /// <summary>
    /// Gets the <see cref="Context"/> associated with the current
    /// <see cref="HttpApplication"/> instance.
    /// </summary>
    public Socket QuerySocket {
      get { return query_socket_; }
    }
  }
}
