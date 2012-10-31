using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// An <see cref="IHistogram"/> taht uses a ramdon sample of a stream.
  /// </summary>
  public class UniformHistogram : AbstractHistogram
  {
    readonly UniformSample sample_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="UniformHistogram"/>
    /// using the specified <see cref="UniformSample"/> sampe.
    /// </summary>
    /// <param name="sample"></param>
    public UniformHistogram(UniformSample sample) {
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
