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

    public static ISample Biased() {
      return new ExponentiallyDecayingSample(kDefaultSampleSize, kDefaultAlpha);
    }

    public static ISample Uniform() {
      return new UniformSample(kDefaultSampleSize);
    }
  }
}
