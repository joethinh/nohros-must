using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An object which samples values.
  /// </summary>
  public interface ISampling
  {
    /// <summary>
    /// Gets a snapshot of the values.
    /// </summary>
    /// <value>A snapshot of the values.</value>
    Snapshot Snapshot { get; }
  }
}
