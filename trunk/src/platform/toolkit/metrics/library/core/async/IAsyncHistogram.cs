using System;

namespace Nohros.Metrics
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
  public interface IAsyncHistogram : IAsyncSummarizable, IAsyncSampling, IMetric,
                                     IHistogram, IAsyncCounted
  {
  }
}
