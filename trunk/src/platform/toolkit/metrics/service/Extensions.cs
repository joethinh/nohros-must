using System;

namespace Nohros.Metrics
{
  internal static class Extensions
  {
    public static MetricName ToMetricName(this MetricNameProto proto) {
      return new MetricName(proto.Name, proto.Type, proto.Name, proto.Scope);
    }
  }
}
