using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A tag interface to indicate that a class is a metric.
  /// </summary>
  public interface IMetric
  {
    /// <summary>
    /// Gets a <see cref="DateTime"/> object representing the current
    /// date and time as seen by the <see cref="IMetric"/> object.
    /// </summary>
    DateTime Now { get; }
  }
}
