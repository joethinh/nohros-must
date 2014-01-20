using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Metrics
{
  public enum MetricValueType : int
  {
    Count,
    OneMinuteRate,
    FiveMinuteRate,
    FifteenMinuteRate,
    Value,
    Min,
    Max,
    Mean,
    StandardDeviation,
    Median,
    Percentile75,
    Percentile95,
    Percentile98,
    Percentile99,
    Percentile999,
    MeanRate
  }
}
