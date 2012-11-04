using System;
using Nohros.Concurrent;

namespace Nohros.Metrics
{
  public sealed class Histograms
  {
    public static UniformHistogram Uniform() {
      return new UniformHistogram(Samples.Uniform());
    }

    public static BiasedHistogram Biased() {
      return new BiasedHistogram(Samples.Biased());
    }

    public static AsyncUniformHistogram Uniform(IExecutor executor) {
      return new AsyncUniformHistogram(Samples.Uniform(), executor);
    }

    public static AsyncBiasedHistogram Biased(IExecutor executor) {
      return new AsyncBiasedHistogram(Samples.Biased(), executor);
    }
  }
}
