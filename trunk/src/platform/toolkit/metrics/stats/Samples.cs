using System;

namespace Nohros.Toolkit.Metrics
{
  public sealed class Samples
  {
    public enum SampleType
    {
      /// <summary>
      /// A uniform sample of 1028 elements, which offer a 99.9% confidence
      /// level with 5% margin of error assuming a normal distribution.
      /// </summary>
      Uniform = 0,

      /// <summary>
      /// An exponentially decaying sample of 1028 elements, which offers a
      /// 99.9% confidence level with a 5% margin of error assuming a normal
      /// distribution, and alpha factor of 0.015, which heavily biases the
      /// sample to the past 5 minutes of measurements.
      /// </summary>
      Biased = 1
    }

    internal const int kDefaultSampleSize = 1028;
    internal const double kDefaultAlpha = 0.015;

    /// <summary>
    /// Creates an <see cref="ExponentiallyDecayingSample"/> that uses a
    /// <see cref="Clock"/> that measures time in nanoseconds and samples
    /// using a reservoir of 1028 items with an 0.015 alpha.
    /// </summary>
    /// <returns></returns>
    public static ExponentiallyDecayingSample Biased() {
      return new ExponentiallyDecayingSample(kDefaultSampleSize, kDefaultAlpha);
    }

    /// <summary>
    /// Creates an <see cref="UniformSample"/> that samples using a reservoir
    /// of 1028 items.
    /// </summary>
    /// <returns></returns>
    public static UniformSample Uniform() {
      return new UniformSample(kDefaultSampleSize);
    }
  }
}
