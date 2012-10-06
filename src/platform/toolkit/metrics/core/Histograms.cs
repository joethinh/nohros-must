using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  public sealed class Histograms
  {
    public static IHistogram Uniform() {
      return new UniformHistogram(Samples.kDefaultSampleSize);
    }

    public static IHistogram Uniform(IExecutor executor) {
      return new UniformHistogram(Samples.kDefaultSampleSize, executor);
    }

    public static IHistogram Biased() {
      return new BiasedHistogram(Samples.kDefaultSampleSize,
        Samples.kDefaultAlpha);
    }

    public static IHistogram Biased(IExecutor executor) {
      return new BiasedHistogram(Samples.kDefaultSampleSize,
        Samples.kDefaultAlpha, executor);
    }
  }
}
