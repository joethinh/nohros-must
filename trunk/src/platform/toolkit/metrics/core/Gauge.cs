using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A gauge value is an instantaneous reading of a particular value.
  /// To instrument a queue's depth, for exemple:
  /// <para>
  /// <example>
  /// <code>
  /// Queue{T} queue = new Queue{T}();
  /// </code>
  /// </example>
  /// </para>
  /// </summary>
  /// <typeparam name="T">The type of the metric's value.</typeparam>
  public abstract class Gauge<T> : IMetric
  {
    /// <summary>
    /// Gets the metric's current value.
    /// </summary>
    public abstract T Value { get; }
  }
}
