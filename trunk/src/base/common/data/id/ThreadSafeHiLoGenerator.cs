using System;

namespace Nohros.Data
{
  /// <summary>
  /// A class that is used to generate integer identity values using the Hi/Lo
  /// algorithm.
  /// </summary>
  public class ThreadSafeHiLoGenerator : HiLoGenerator
  {
    readonly object lock_;

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
    public ThreadSafeHiLoGenerator(NextHiDelegate next_hi, int max_lo,
      string key)
      : base(next_hi, max_lo, key) {
      lock_ = new object();
    }
    #endregion

    /// <summary>
    /// Generate the next available identity.
    /// </summary>
    /// <returns>
    /// The next available identity value.
    /// </returns>
    public override long Generate() {
      lock (lock_) {
        return base.Generate();
      }
    }
  }
}
