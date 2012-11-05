using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// Represents the instantaneous value of a metric.
  /// </summary>
  public struct MetricValue
  {
    readonly string name_;
    readonly double value_;

    #region .ctor
    public MetricValue(string name, double value) {
      name_ = name;
      value_ = value;
    }
    #endregion

    /// <summary>
    /// Gets the instantaneous metric's value
    /// </summary>
    public double Value {
      get { return value_; }
    }

    /// <summary>
    /// Gets the name associated with the instantaneous values.
    /// </summary>
    /// <remarks>
    /// This is not the name of the metric, bu the name of the value associated
    /// with the metric. Ex. "99percentile", "min", "max".
    /// </remarks>
    public string Name {
      get { return name_; }
    }
  }
}
