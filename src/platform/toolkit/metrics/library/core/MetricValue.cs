using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// Represents the instantaneous value of a metric.
  /// </summary>
  public struct MetricValue
  {
    readonly string name_;
    readonly string unit_;
    readonly double value_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricName"/> class using
    /// the specified metric name, value and unit type.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    public MetricValue(string name, double value)
      : this(name, value, string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricName"/> class using
    /// the specified metric name, value and unit type.
    /// </summary>
    /// <param name="name">
    /// The name of the metric.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    /// <param name="unit">
    /// The type of unit of the <paramref name="value"/>.
    /// </param>
    public MetricValue(string name, double value, string unit) {
      name_ = name;
      value_ = value;
      unit_ = unit;
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

    public string Unit {
      get { return unit_; }
    }
  }
}
