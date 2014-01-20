using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// Represents the instantaneous value of a metric.
  /// </summary>
  public struct MetricValue
  {
    readonly string unit_;
    readonly double value_;
    readonly int value_type_;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class using
    /// the specified metric type, value and unit type.
    /// </summary>
    /// <param name="value_type">
    /// The name of the metric.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    public MetricValue(MetricValueType value_type, double value)
      : this((int) value_type, value, string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class using
    /// the specified metric type, value and unit type.
    /// </summary>
    /// <param name="value_type">
    /// The name of the metric.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    public MetricValue(int value_type, double value)
      : this(value_type, value, string.Empty) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class using
    /// the specified metric value type, value and unit type.
    /// </summary>
    /// <param name="value_type">
    /// The type of the metric value.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    /// <param name="unit">
    /// The type of unit of the <paramref name="value"/>.
    /// </param>
    /// <see cref="MetricValueType"/>
    public MetricValue(MetricValueType value_type, double value, string unit) {
      value_type_ = (int) value_type;
      value_ = value;
      unit_ = unit;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricValue"/> class using
    /// the specified metric value type, value and unit type.
    /// </summary>
    /// <param name="value_type">
    /// The type of the metric value.
    /// </param>
    /// <param name="value">
    /// The metric's value
    /// </param>
    /// <param name="unit">
    /// The type of unit of the <paramref name="value"/>.
    /// </param>
    /// <see cref="MetricValueType"/>
    public MetricValue(int value_type, double value, string unit) {
      value_type_ = value_type;
      value_ = value;
      unit_ = unit;
    }

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
    /// This is not the name of the metric, but the name of the value associated
    /// with the metric. Ex. "99percentile", "min", "max".
    /// </remarks>
    public int Type {
      get { return value_type_; }
    }

    public string Unit {
      get { return unit_; }
    }
  }
}
