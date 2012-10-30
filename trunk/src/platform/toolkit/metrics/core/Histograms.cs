using System;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  public sealed class Histograms
  {
    public static IHistogram Uniform() {
      return new UniformHistogram(Samples.Uniform());
    }

    public static IHistogram Biased() {
      return new BiasedHistogram(Samples.Biased());
    }
  }
}
