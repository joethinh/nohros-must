using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// An <see cref="IHistogram"/> that uses an exponentially decaying sample.
  /// </summary>
  public class BiasedHistogram : AbstractHistogram
  {
    readonly ExponentiallyDecayingSample sample_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="BiasedHistogram"/> using
    /// the specified <see cref="ExponentiallyDecayingSample"/> sample.
    /// </summary>
    /// <param name="sample">
    /// A <see cref="ExponentiallyDecayingSample"/> sample.
    /// </param>
    public BiasedHistogram(ExponentiallyDecayingSample sample) {
      sample_ = sample;
    }
    #endregion

    /// <inheritdoc/>
    public override void Update(long value) {
      long timestamp = TimeUnitHelper
        .ToSeconds(Clock.CurrentTimeMilis, TimeUnit.Miliseconds);
      Update(value, timestamp);
    }

    /// <summary>
    /// Adds an old value with fixed timestamp to the sample.
    /// </summary>
    /// <param name="value">
    /// The value to be added.
    /// </param>
    /// <param name="timestamp">
    /// The epoch timestamp of <paramref name="value"/> in seconds.
    /// </param>
    public void Update(long value, long timestamp) {
      base.Update(value);
      sample_.Update(value, timestamp);
    }

    /// <inheritdoc/>
    public override Snapshot Snapshot {
      get { return sample_.Snapshot; }
    }
  }
}
