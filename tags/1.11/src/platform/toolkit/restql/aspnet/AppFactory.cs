using System;
using Nohros.Concurrent;
using Nohros.Configuration;
using Nohros.Extensions;
using Nohros.Logging;
using ZMQ;

namespace Nohros.RestQL
{
  public class HttpQueryApplicationFactory
  {
    readonly ISettings settings_;

    #region .ctor
    public HttpQueryApplicationFactory(Settings settings) {
      settings_ = settings;
    }
    #endregion

    /// <summary>
    /// Creates an instance of the <see cref="HttpQueryApplication"/> class.
    /// </summary>
    /// <returns>
    /// The newly created <see cref="HttpQueryApplication"/> object.
    /// </returns>
    public HttpQueryApplication CreateQueryApplication() {
      var background_thread_factory = new BackgroundThreadFactory();
      var context = new Context();
      var app = new HttpQueryApplication(settings_, context,
        background_thread_factory);
      return app;
    }

    /// <summary>
    /// Configures the logger that should be used by the application.
    /// </summary>
    public void ConfigureLogger() {
      try {
        IProviderNode provider = settings_.Providers
          .GetProviderNode(Strings.kLoggingProviderName);

        ILogger logger = RuntimeTypeFactory<ILoggerFactory>
          .CreateInstanceFallback(provider, settings_)
          .CreateLogger(provider.Options.ToDictionary());
        HttpQueryLogger.ForCurrentProcess.Logger = logger;
      } catch {
        // fails silently.
      }
    }
  }
}
