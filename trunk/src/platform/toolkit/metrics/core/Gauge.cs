using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A gauge value is an instantaneous reading of a particular value.
  /// To instrument a queue's depth, for exemple:
  /// <para>
  /// <example>
  /// <code>

  /// </code>
  /// </example>
  /// </para>
  /// </summary>
  /// <typeparam name="T">The type of the metric's value.</typeparam>
  public abstract class Gauge<T>
  {
    /// <summary>
    /// Gets the metric's current value.
    /// </summary>
    public abstract T Value { get; }
  }
}
