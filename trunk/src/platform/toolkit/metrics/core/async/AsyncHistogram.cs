using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  public class AsyncHistogram : IAsyncHistogram
  {
    readonly Mailbox<RunnableDelegate> async_tasks_mailbox_;
    readonly Histogram histogram_;

    #region .ctor
    protected AsyncHistogram(Histogram histogram, IExecutor executor) {
      histogram_ = histogram;
      async_tasks_mailbox_ = new Mailbox<RunnableDelegate>(
        runnable => runnable(), executor);
    }
    #endregion

    /// <inheritdoc/>
    public virtual void GetSnapshot(SnapshotCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(histogram_.Snapshot, now));
    }

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

    public virtual void Update(long value) {
      async_tasks_mailbox_.Send(() => histogram_.Update(value));
    }
  }
}
