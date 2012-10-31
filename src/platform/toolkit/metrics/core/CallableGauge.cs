using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A <see cref="Gauge{T}"/> that computes its value using a
  /// <see cref="CallableDelegate{T}"/>.
  /// </summary>
  /// <typeparam name="T">
  /// The type of <see cref="Value"/>.
  /// </typeparam>
  public class CallableGauge<T> : Gauge<T>
  {
    readonly CallableDelegate<T> callable_;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallableGauge{T}"/>
    /// by using the specified <see cref="CallableDelegate{T}"/>.
    /// </summary>
    /// <param name="callable">
    /// A <see cref="CallableDelegate{T}"/> that is used to compute the gauge
    /// values.
    /// </param>
    public CallableGauge(CallableDelegate<T> callable) {
      callable_ = callable;
    }

    /// <inheritdoc/>
    public override T Value {
      get { return callable_(); }
    }
  }
}
