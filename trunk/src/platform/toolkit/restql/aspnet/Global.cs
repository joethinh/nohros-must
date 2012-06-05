using System;
using System.Web;

namespace Nohros.Toolkit.RestQL
{
  public class Global : HttpApplication
  {
    static readonly QueryServer server_;

    #region .ctor
    /// <summary>
    /// Initializes application static variables. This is the application
    /// main().
    /// </summary>
    static Global() {
      server_ = new QueryServer.Builder().Build();
    }
    #endregion

    /// <summary>
    /// Gets a <see cref="QueryProcessor"/> object that can be used to process
    /// restql queries.
    /// </summary>
    public static QueryServer QueryServer {
      get { return server_; }
    }
  }
}
