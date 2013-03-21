using System;
using System.Collections.Generic;

namespace Nohros.Metrics.Data
{
  public interface IMetricsRepositoryFactory
  {
    /// <summary>
    /// Create a new instance of the <see cref="IMetricsRepository"/>
    /// using the specified options.
    /// </summary>
    /// <param name="options">
    /// A collection of key/value pairs containing the user configured options.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IMetricsRepository"/> object.
    /// </returns>
    IMetricsRepository CreateMetricsDataProvider(
      IDictionary<string, string> options);
  }
}
