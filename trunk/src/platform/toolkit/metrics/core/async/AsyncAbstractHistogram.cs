using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  public abstract class AbstractAsyncHistogram : IAsyncHistogram
  {
    protected readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    readonly IHistogram histogram_;

    #region .ctor
    protected AbstractAsyncHistogram(IExecutor executor, IHistogram histogram) {
      histogram_ = histogram;
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(
        runnable => runnable(), executor);
    }
    #endregion

    /// <inheritdoc/>
    public abstract void GetSnapshot(SnapshotCallback callback);

    /// <inheritdoc/>
    public virtual void GetMax(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(histogram_.Max, now));
    }

    /// <inheritdoc/>
    public virtual void GetMin(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(histogram_.Min, now));
    }

    /// <inheritdoc/>
    public virtual void GetMean(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(histogram_.Mean, now));
    }

    /// <inheritdoc/>
    public virtual void GetStandardDeviation(DoubleMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(
        () => callback(histogram_.StandardDeviation, now));
    }

    /// <inheritdoc/>
    public virtual void GetCount(LongMetricCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(histogram_.Count, now));
    }

    /// <inheritdoc/>
    public abstract void Update(long value);
  }
}
