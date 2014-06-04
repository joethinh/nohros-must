using System;
using System.Timers;

namespace Nohros.Metrics
{
  public class MetricsReporter
  {
    readonly IAsyncMetricsRegistry registry_;
    readonly ISettings settings_;
    readonly System.Timers.Timer timer_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsReporter"/>
    /// class using the specified application settings.
    /// </summary>
    /// <param name="settings">
    /// A <see cref="ISettings"/> object containig the user configured settings.
    /// </param>
    public MetricsReporter(ISettings settings, IAsyncMetricsRegistry registry) {
      settings_ = settings;
      registry_ = registry;
      timer_ = new System.Timers.Timer(settings_.ReportingInterval) {
        AutoReset = true
      };
      timer_.Elapsed += Run;
    }
    #endregion

    void Run(object sender, ElapsedEventArgs e) {
      registry_.
    }

    public void Start() {
      timer_.Start();
    }

    public void Stop() {
      timer_.Stop();
    }

    /// <summary>
    /// Gets the <see cref="IAsyncMetricsRegistry"/> associated with the app.
    /// </summary>
    public IAsyncMetricsRegistry Registry {
      get { return registry_; }
    }
  }
}
