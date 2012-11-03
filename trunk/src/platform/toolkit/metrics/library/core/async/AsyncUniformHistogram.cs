using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  public class AsyncUniformHistogram : AbstractAsyncHistogram
  {
    // This class is a way to to use the logic implemented by the
    // AbstractHistogram without exposing its synchronous methods to externals.
    class Histogram : AbstractHistogram
    {
      public override Snapshot Snapshot {
        get { throw new NotImplementedException(); }
      }
    }

    readonly UniformSample sample_;

    #region .ctor
    public AsyncUniformHistogram(UniformSample sample, IExecutor executor)
      : base(executor, new Histogram()) {
      sample_ = sample;
    }
    #endregion

    /// <inheritdoc/>
    public override void Update(long value) {
      async_tasks_mailbox_.Send(() => {
        Update(value);
        sample_.Update(value);
      });
    }

    /// <inheritdoc/>
    public override void GetSnapshot(SnapshotCallback callback) {
      var now = DateTime.Now;
      async_tasks_mailbox_.Send(() => callback(sample_.Snapshot, now));
    }
  }
}
