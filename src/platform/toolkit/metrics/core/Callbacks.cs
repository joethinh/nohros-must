using System;

namespace Nohros.Toolkit.Metrics
{
  public delegate void DoubleMetricCallback(double d, DateTime timestamp);
  public delegate void LongMetricCallback(long l, DateTime timestamp);
  public delegate void SnapshotCallback(Snapshot snapshot, DateTime timestamp);
}
