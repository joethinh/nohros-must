using System;

namespace Nohros.Metrics
{
  public delegate void DoubleMetricCallback(double d, DateTime timestamp);
  public delegate void LongMetricCallback(long l, DateTime timestamp);
  public delegate void SnapshotCallback(Snapshot snapshot, DateTime timestamp);
  public delegate void MeteredCallback(IMetered metered);
  public delegate void CountedCallback(ICounted counted);
}
