using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  /// <summary>
  /// An incrementing and decrementing counter metric.
  /// </summary>
  public class Counter : IMetric
  {
    readonly Mailbox<long> async_update_mailbox_;
    long count_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Counter"/> class that
    /// uses a thread pool to perform the counter update.
    /// </summary>
    public Counter() : this(Executors.ThreadPoolExecutor()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Counter"/> class that
    /// uses the specified executor to perform the counter updates (
    /// increment/decrement).
    /// </summary>
    /// <param name="executor">
    /// A <see cref="IExecutor"/> that is used to execute the sample updates.
    /// </param>
    /// <remarks>
    /// The use of the executor returned by the method
    /// <see cref="Executors.SameThreadExecutor"/> is not encouraged, because
    /// the executor does not returns until the execution list is empty and,
    /// this can cause significant pauses in the thread that is executing the
    /// sample update.
    /// </remarks>
    public Counter(IExecutor executor) {
      count_ = 0;
      async_update_mailbox_ = new Mailbox<long>(AsyncUpdate, executor);
    }
    #endregion

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      callback(new[] { new MetricValue("count", Count) }, context);
    }

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
      async_update_mailbox_.Send(n);
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
      async_update_mailbox_.Send(-n);
    }

    void AsyncUpdate(long delta) {
      count_ += delta;
    }

    /// <summary>
    /// Gets the counter current value.
    /// </summary>
    public long Count {
      get { return count_; }
    }
  }
}
