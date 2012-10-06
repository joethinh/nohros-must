using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A metric which calculates the distribution of a value.
  /// </summary>
  /// <remarks>
  /// Accurately computing running variance
  /// <para>
  ///  http://www.johndcook.com/standard_deviation.html
  /// </para>
  /// </remarks>
  public interface IHistogram : ISummarizable, ISampling, IMetric
  {
    /// <summary>
    /// Adds a recorded value.
    /// </summary>
    void Update(long value);

    /// <summary>
    /// Get the number of values recorded.
    /// </summary>
    /// <returns>The number of values recorded.</returns>
    long Count { get; }
  }
}
