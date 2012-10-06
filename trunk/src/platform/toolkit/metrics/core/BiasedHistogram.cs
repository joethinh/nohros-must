using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  public class BiasedHistogram : AbstractHistogram
  {
    readonly Mailbox<long> async_update_mailbox_;
    readonly ExponentiallyDecayingSample sample_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="BiasedHistogram"/>
    /// using <paramref name="sample_size"/> as the size of the sample.
    /// </summary>
    /// <param name="sample_size">
    /// The number of elements to sample.
    /// </param>
    public BiasedHistogram(int sample_size, double alpha)
      : this(sample_size, alpha, Executors.ThreadPoolExecutor()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BiasedHistogram"/>
    /// using <paramref name="sample_size"/> as the size of the sample and
    /// the specified executor.
    /// </summary>
    /// <param name="sample_size">
    /// The number of elements to sample.
    /// </param>
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
    public BiasedHistogram(int sample_size, double alpha, IExecutor executor) {
      // the sample should be update in the same thread that is updating the
      // histogram, to guarantee the consistency of the snapshot.
      sample_ = new ExponentiallyDecayingSample(sample_size, alpha,
        Executors.SameThreadExecutor());
      async_update_mailbox_ = new Mailbox<long>(AsyncUpdate, executor);
    }
    #endregion

    /// <summary>
    /// Adds a recorded value.
    /// </summary>
    /// <remarks>
    /// The update operation is performed asynchrounsly and is thread-safe.
    /// </remarks>
    public override void Update(long value) {
      async_update_mailbox_.Send(value);
    }

    public void AsyncUpdate(long value) {
      base.Update(value);
      sample_.Update(value);
    }

    /// <inheritdoc/>
    public override Snapshot Snapshot {
      get { return sample_.Snapshot; }
    }
  }
}
