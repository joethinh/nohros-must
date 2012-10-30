using System;

namespace Nohros.Toolkit.Metrics
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
      base.Update(value);
      sample_.Update(value);
    }

    /// <inheritdoc/>
    public override Snapshot Snapshot {
      get { return sample_.Snapshot; }
    }
  }
}
