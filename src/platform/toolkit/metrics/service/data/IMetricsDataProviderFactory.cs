using System;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  public interface IMetricsDataProviderFactory
  {
    /// <summary>
    /// Create a new instance of the <see cref="IMetricsDataProvider"/>
    /// using the specified options.
    /// </summary>
    /// <param name="options">
    /// A collection of key/value pairs containing the user configured options.
    /// </param>
    /// <returns>
    /// The newly created <see cref="IMetricsDataProvider"/> object.
    /// </returns>
    IMetricsDataProvider CreateMetricsDataProvider(
      IDictionary<string, string> options);
  }
}
