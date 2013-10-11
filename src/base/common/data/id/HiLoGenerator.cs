using System;

namespace Nohros.Data
{
  /// <summary>
  /// An implementation of the <see cref="IHiLoGenerator"/> class.
  /// </summary>
  public class HiLoGenerator : IHiLoGenerator
  {
    class FullHiloRange : IHiLoRange
    {
      #region .ctor
      public FullHiloRange() {
        High = 1;
        MaxLow = 0;
      }
      #endregion

      public long High { get; private set; }
      public int MaxLow { get; private set; }
    }

    readonly NextHighDelegate next_high_;
    IHiLoRange hi_lo_range_;
    long lo_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="HiLoGenerator"/> class
    /// by using the given next high delegate and max lo value.
    /// </summary>
    /// <param name="next_high">
    /// A <see cref="CallableDelegate{T}"/> that is used to get the next High
    /// value.
    /// </param>
    public HiLoGenerator(NextHighDelegate next_high) {
      if (next_high == null) {
        throw new ArgumentNullException("next_high");
      }

      next_high_ = next_high;
      hi_lo_range_ = new FullHiloRange();
      lo_ = MaxHigh + 1;
    }
    #endregion

    /// <inheritdoc/>
    public virtual long Generate(string key) {
      if (key == null) {
        throw new ArgumentNullException("key");
      }

      if (lo_ > MaxHigh) {
        hi_lo_range_ = next_high_(key);
        lo_ = 0;
      }
      return hi_lo_range_.High + lo_++;
    }

    /// <inheritdoc/>
    public virtual long Generate() {
      return Generate(string.Empty);
    }

    long MaxHigh {
      get { return hi_lo_range_.High + hi_lo_range_.MaxLow; }
    }
  }
}
