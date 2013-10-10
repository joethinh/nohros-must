using System;

namespace Nohros.Data
{
  /// <summary>
  /// A class that is used to generate integer identity values using the High/Lo
  /// algorithm.
  /// </summary>
  public class ThreadSafeHiLoGenerator : HiLoGenerator, IHiLoGenerator
  {
    readonly object lock_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="HiLoGenerator"/> class
    /// by using the given next hi delegate and max lo value.
    /// </summary>
    /// <param name="next_high">
    /// A <see cref="CallableDelegate{T}"/> that is used to get the next High
    /// value.
    /// </param>
    public ThreadSafeHiLoGenerator(NextHighDelegate next_high)
      : base(next_high) {
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
