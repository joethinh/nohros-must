using System;
using System.Collections.Generic;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// A factory for the <see cref="IMetricsReporter"/> class.
  /// </summary>
  public interface IMetricsReporterFactory
  {
    /// <summary>
    /// Creates a new instance of the <see cref="IMetricsReporter"/> using the
    /// specified options.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> contianing the configured
    /// options for the reporter.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IMetricsReporter"/> object.
    /// </returns>
    IMetricsReporter CreateMetricsReporter(IDictionary<string, string> options);
  }
}
