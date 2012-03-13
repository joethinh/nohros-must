using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A ramdom sample of a stream of <see cref="long"/>'s. Uses Vitter's
  /// Algorithm R to produce a statically representative sample.
  /// </summary>
  /// <remarks>
  /// Ramdom Sampling with a Resovoir -
  ///   http://www.cs.umd.edu/~samir/498/vitter.pdf
  /// </remarks>
  public class UniformSample : ISample
  {
    static readonly Random random_;
    static readonly int kBitsPerLong = 63;
    readonly AtomicLong count_;
    readonly AtomicLongArray values_;

    #region .ctor
    /// <summary>
    /// Perform the initialization of the static members.
    /// </summary>
    static UniformSample() {
      random_ = new Random();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniformSample"/> class.
    /// </summary>
    /// <param name="resevoir_size">The number of samples to keep in the
    /// sampling resevoir.</param>
    public UniformSample(int resevoir_size) {
      values_ = new AtomicLongArray(resevoir_size);
      
    }
    #endregion

    /// <inheritdoc/>
    public void Clear() {
      for (int i = 0, j = values_.Length; i < j; i++) {
        values_.Exchange(i, 0);
      }
      count_.Exchange(0);
    }

    /// <inheritdoc/>
    public int Size {
      get {
        long c = (long)count_;
        return (c > values_.Length) ? values_.Length : (int)c;
      }
    }

    /// <inheritdoc/>
    public void Update(long value) {
      long c = (long)count_.Increment();
      if (c <= values_.Length) {
        values_.Exchange((int)c - 1, value);
      } else {
        long r = NextLong(c);
        if (r < values_.Length) {
          values_.Exchange((int)r, value);
        }
      }
    }

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get {
        return new Snapshot((long[])values_);
      }
    }

    /// <summary>
    /// Gets a pseudo-random long uniformly between 0 and n-1.
    /// </summary>
    /// <param name="n"></param>
    /// <returns>A value select randomly form the range <c>[0..n]</c></returns>
    static long NextLong(long n) {
      long bits, val;
      do {
        long next_random_long =
          (long)((random_.NextDouble() * 2.0 - 1.0) * long.MaxValue);
        bits = next_random_long & (~(1L << kBitsPerLong));
        val = bits % n;
      } while (bits - val + (n - 1) < 0L);
      return val;
    }
  }
}
