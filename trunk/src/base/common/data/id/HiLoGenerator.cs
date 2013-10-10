using System;

namespace Nohros.Data
{
  /// <summary>
  /// A class that is used to generate integer identity values using the Hi/Lo
  /// algorithm.
  /// </summary>
  public class HiLoGenerator
  {
    /// <summary>
    /// A method that is called to get the next High value for the given key.
    /// </summary>
    /// <param name="key">
    /// A string that identifies the object to get the next hi value(ex. The
    /// name of a table in a RDBMS).
    /// </param>
    public delegate long NextHiDelegate(string key);

    readonly string key_;

    readonly int max_lo_;
    readonly NextHiDelegate next_hi_;
    long hi_;
    long lo_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="HiLoGenerator"/> class
    /// by using the given next hi delegate and max lo value.
    /// </summary>
    /// <param name="next_hi">
    /// A <see cref="CallableDelegate{T}"/> that is used to get the next Hi
    /// value.
    /// </param>
    /// <param name="max_lo">
    /// The maximum number of lo values to be generated.
    /// </param>
    /// <param name="key">
    /// A string that identifies the object to get the next hi value(ex. The
    /// name of a table in a RDBMS).
    /// </param>
    public HiLoGenerator(NextHiDelegate next_hi, int max_lo, string key) {
      if (next_hi == null) {
        throw new ArgumentNullException("next_hi");
      }

      if (key == null) {
        throw new ArgumentNullException("key");
      }

      if (max_lo <= 0) {
        throw new ArgumentOutOfRangeException("max_lo",
          "max_lo should be greater than zero");
      }
      next_hi_ = next_hi;
      max_lo_ = max_lo;
      lo_ = max_lo + 1;
      hi_ = 0;
      key_ = key;
    }
    #endregion

    /// <summary>
    /// Generate the next available identity.
    /// </summary>
    /// <returns>
    /// The next available identity value.
    /// </returns>
    public virtual long Generate() {
      if (lo_ > max_lo_) {
        hi_ = next_hi_(key_);
        lo_ = 0;
      }
      return hi_ + lo_++;
    }
  }
}
