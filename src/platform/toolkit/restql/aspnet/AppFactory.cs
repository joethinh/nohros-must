using System;
using Nohros.Concurrent;
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
      var background_thread_factory = new BackgroundThreadFactory();
      var context = new Context();
      var app = new HttpQueryApplication(settings_, context,
        background_thread_factory);
      return app;
    }
  }
}
