using System;

namespace Nohros.Metrics.Reporting
{
  public interface IMetricsReporter
  {
    /// <summary>
    /// Stops the reporter and deallocates any associated resources.
    /// </summary>
    void Shutdown();
  }
}
