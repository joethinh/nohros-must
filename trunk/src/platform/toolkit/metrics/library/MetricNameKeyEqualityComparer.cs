using System;
using System.Collections.Generic;

namespace Nohros.Metrics
{
  internal class MetricNameKeyEqualityComparer : IEqualityComparer<MetricName>
  {
    public bool Equals(MetricName x, MetricName y) {
      return x.Equals(y);
    }

    public int GetHashCode(MetricName obj) {
      return obj.Key.GetHashCode();
    }
  }
}
