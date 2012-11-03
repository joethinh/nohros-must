using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  public class AsyncBiasedHistogram : AbstractAsyncHistogram
  {
    // This class is a way to to use the logic implemented by the
    // AbstractHistogram without exposing its synchronous methods to externals.
    class Histogram : AbstractHistogram
    {
      public override Snapshot Snapshot {
        get { throw new NotImplementedException(); }
      }
    }

    readonly ExponentiallyDecayingSample sample_;

    #region .ctor
    public AsyncBiasedHistogram(ExponentiallyDecayingSample sample,
      IExecutor executor) : base(executor, new Histogram()) {
      sample_ = sample;
    }
    #endregion

    /// <inheritdoc/>
    public override void Update(long value) {
      long timestamp = TimeUnitHelper
        .ToSeconds(Clock.CurrentTimeMilis, TimeUnit.Miliseconds);
      async_tasks_mailbox_.Send(() => {
        Update(value);
        sample_.Update(value, timestamp);
      });
    }

    /// <inheritdoc/>
    public override void GetSnapshot(SnapshotCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(sample_.Snapshot, now));
    }
  }
}
