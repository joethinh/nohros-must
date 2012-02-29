using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Nohros.Data.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A metric which calculates the distribution of a value.
  /// </summary>
  /// <remarks>
  /// Accurately computing running variance
  /// <para>
  ///  http://www.johndcook.com/standard_deviation.html
  /// </para>
  /// </remarks>
  public partial class Histogram : ISummarizable, ISampling, IMetric
  {
    internal const int kDefaultSampleSize = 1028;
    internal const double kDefaultAlpha = 0.015;

    enum SampleType
    {
      /// <summary>
      /// A uniform sample of 1028 elements, which offer a 99.9% confidence
      /// level with 5% margin of error assuming a normal distribution.
      /// </summary>
      UNIFORM = 0,

      /// <summary>
      /// An exponentially decaying sample of 1028 elements, which offers a
      /// 99.9% confidence level with a 5% margin of error assuming a normal
      /// distribution, and alpha factor of 0.015, which heavily biases the
      /// sample to the past 5 minutes of measurements.
      /// </summary>
      BIASED = 1
    }

    [ThreadStatic]
    /// <summary>
    /// Cache arrays for the vairance calculation, so as to avoid memory
    /// allocation.
    /// </summary>
    protected static double[] thread_static_initial_values_;

    readonly ISample sample_;
    readonly AtomicLong min_;
    readonly AtomicLong max_;
    readonly AtomicLong sum_;
    readonly AtomicLong count_;
    readonly AtomicReference<double[]> variance_;

    #region .ctor
    /// <summary>
    /// Initialize a new instance of the <see cref="Histogram"/> class by using
    /// the specified sample data.
    /// </summary>
    /// <param name="sample"></param>
    public Histogram(ISample sample) {
      sample_ = sample;
      min_ = new AtomicLong();
      max_ = new AtomicLong();
      sum_ = new AtomicLong();
      count_ = new AtomicLong();
      variance_ = new AtomicReference<double[]>(new double[] { -1, 0 }); //M,S
      thread_static_initial_values_ = new double[2];
    }
    #endregion

    /// <summary>
    /// Clears all the recorded values
    /// </summary>
    public void Clear() {
      sample_.Clear();
      count_.Exchange(0);
      max_.Exchange(0);
      min_.Exchange(0);
      sum_.Exchange(0);
      variance_.Exchange(new double[] { -1, 0 });
    }

    /// <summary>
    /// Adds a recorded value
    /// </summary>
    /// <param name="value">The length of the value.</param>
    public void Update(int value) {
      Update((long)value);
    }

    /// <summary>
    /// Adds a recorded value.
    /// </summary>
    public void Update(long value) {
      count_.Increment();
      sample_.Update(value);
      SetMax(value);
      SetMin(value);
      sum_.Add(value);
      UpdateVariance(value);
    }

    double Variance() {
      long count = Count;
      if (count <= 1) {
        return 0.0;
      }
      return variance_.Value[1] / (count - 1);
    }

    /// <summary>
    /// Set the maximum recorded value value.
    /// </summary>
    /// <param name="value">The value that is potentially the maximum value.
    /// </param>
    void SetMax(long value) {
      bool done = false;
      while (!done) {
        long current_max = (long)max_;

        // check if we already have the maximum value.
        done = current_max >= value;
        if (!done) {
          // atomically set the maximum value if it is still the value that we
          // use in the check above.
          min_.CompareExchange(current_max, value);
        }
      }
    }

    /// <summary>
    /// Set the maximum recorded value value.
    /// </summary>
    /// <param name="value">The value that is potentially the minimum value.
    /// </param>
    /// <remarks>If the specified value is greater</remarks>
    void SetMin(long value) {
      bool done = false;
      while (!done) {
        long current_min = (long)min_;

        // check if we already have the minimum value.
        done = current_min <= value;
        if (!done) {
          // atomically set the minimum value if it is still the value that we
          // use in the check above.
          min_.CompareExchange(current_min, value);
        }
      }
    }

    /// <summary>
    /// Adds the specified long value to the variance calculation.
    /// </summary>
    /// <param name="value">The be added to the variance calculation.</param>
    void UpdateVariance(long value) {
      bool done = false;
      while (!done) {
        double[] old_values = variance_.Value;
        double[] new_values = thread_static_initial_values_;
        if (old_values[0] == -1) {
          new_values[0] = value;
          new_values[1] = 0;
        } else {
          double old_m = old_values[0];
          double old_s = old_values[1];

          double new_m = old_m + ((value - old_m) / Count);
          double new_s = old_s + ((value - old_m) * (value - new_m));

          new_values[0] = new_m;
          new_values[1] = new_s;
        }
        double[] compare_old_values =
          variance_.CompareExchange(old_values, new_values);

        // if we are done, recycle the old value into the cache.
        if (done = (compare_old_values == old_values)) {
          thread_static_initial_values_ = old_values;
        }
      }
    }

    /// <summary>
    /// Get the number of values recorded.
    /// </summary>
    /// <returns>The number of values recorded.</returns>
    public long Count {
      get { return (long)count_; }
    }

    /// <inheritdoc/>
    public double Max {
      get {
        return (Count > 0) ? max_.Value : 0.0;
      }
    }

    /// <inheritdoc/>
    public double Min {
      get {
        return (Count > 0) ? max_.Value : 0.0;
      }
    }

    /// <inheritdoc/>
    public double Mean {
      get {
        long count = Count;
        if (count > 0) {
          return sum_.Value / (double)count;
        }
        return 0.0;
      }
    }

    /// <inheritdoc/>
    public double StandardDeviation {
      get {
        return (Count > 0) ? Math.Sqrt(Variance()) : 0.0;
      }
    }

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get { return sample_.Snapshot; }
    }
  }
}