using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Metrics.Reporting
{
  public interface IPollingMetricsReporterFactory
  {
    /// <summary>
    /// Creates a new instance of the <see cref="IPollingMetricsReporter"/> class by
    /// using the specified reporter options
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the configured
    /// options for the reporter.
    /// </param>
    /// <returns>
    /// THe newly created <see cref="IPollingMetricsReporter"/> object.
    /// </returns>
    IPollingMetricsReporter CreatePollingReporter(
      IDictionary<string, string> options);
  }
}
