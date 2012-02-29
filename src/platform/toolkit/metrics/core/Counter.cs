using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An incrementing and decrementing counter metric.
  /// </summary>
  public class Counter : IMetric
  {
    private readonly AtomicLong count_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Counter"/> class.
    /// </summary>
    public Counter() {
      this.count_ = new AtomicLong(0);
    }
    #endregion

    /// <summary>
    /// Increment the counter by one.
    /// </summary>
    public void Increment() {
      Increment(1);
    }

    /// <summary>
    /// Increments the counter by <paramref name="n"/>.
    /// </summary>
    /// <param name="n">The amount by which the counter will be increased.
    /// </param>
    public void Increment(long n) {
      count_.Add(1);
    }

    /// <summary>
    /// Decrements the counter by one.
    /// </summary>
    public void Decrement() {
      Decrement(1);
    }

    /// <summary>
    /// Decrements the counter by <paramref name="n"/>
    /// </summary>
    /// <param name="n">The amount by which the counter will be increased.
    /// </param>
    public void Decrement(long n) {
      count_.Add(-n);
    }

    /// <summary>
    /// Gets the counter current value.
    /// </summary>
    public long Count {
      get { return count_.Value; }
    }

    /// <summary>
    /// Resets the couter to zero.
    /// </summary>
    public void Clear() {
      count_.Exchange(0);
    }
  }
}
