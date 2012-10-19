using System;
using ZMQ;

namespace Nohros.Toolkit.RestQL
{
  public class HttpQueryApplicationFactory
  {
    readonly ISettings settings_;

    #region .ctor
    public HttpQueryApplicationFactory(Settings settings) {
      settings_ = settings;
    }
    #endregion

    public HttpQueryApplication CreateQueryApplication() {
      var app = new HttpQueryApplication(settings_, new Context(1));
      app.Run();
      return app;
    }
  }
}
