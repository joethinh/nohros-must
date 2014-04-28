using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  /// <summary>
  /// An incrementing and decrementing counter metric.
  /// </summary>
  public class AsyncCounter : IMetric, IAsyncCounter, ICounted
  {
    readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    long count_;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCounter"/> class that
    /// uses a thread pool to perform the counter update.
    /// </summary>
    public AsyncCounter() : this(Executors.ThreadPoolExecutor()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncCounter"/> class that
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
    public AsyncCounter(IExecutor executor) {
      count_ = 0;
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(Run, executor);
    }

    void Run(RunnableDelegate runnable) {
      runnable();
    }

    public void Report<T>(MetricReportCallback<T> callback, T context) {
      var value = new MetricValue(MetricValueType.Count, Count);
      var set = new MetricValueSet(this, value);
      callback(set, context);
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
      async_tasks_mailbox_.Send(() => Update(n));
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
      async_tasks_mailbox_.Send(() => Update(-n));
    }

    void Update(long delta) {
      count_ += delta;
    }

    public void GetCount(LongMetricCallback callback) {
      DateTime now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(count_, now));
    }

    public void Increment(CountedCallback callback) {
      Increment(1, callback);
    }

    public void Increment(long n, CountedCallback callback) {
      async_tasks_mailbox_.Send(() => {
        Update(n);
        callback(this);
      });
    }

    public void Decrement(CountedCallback callback) {
      Decrement(1);
    }

    public void Decrement(long n, CountedCallback callback) {
      async_tasks_mailbox_.Send(() => {
        Update(-n);
        callback(this);
      });
    }

    /// <summary>
    /// Gets the counter current value.
    /// </summary>
    long ICounted.Count {
      get { return count_; }
    }

    long Count {
      get { return count_; }
    }
  }
}
