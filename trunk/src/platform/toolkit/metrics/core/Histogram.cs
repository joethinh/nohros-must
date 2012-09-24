using System;
using Nohros.Concurrent;

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
    readonly ISample sample_;
    readonly AtomicReference<double[]> variance_;
    AtomicLong count_;
    AtomicLong max_;
    AtomicLong min_;
    AtomicLong sum_;

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
      variance_ = new AtomicReference<double[]>(new double[] {-1, 0}); //M,S
    }
    #endregion

    /// <inheritdoc/>
    public Snapshot Snapshot {
      get { return sample_.Snapshot; }
    }

    /// <inheritdoc/>
    public double Max {
      get { return (Count > 0) ? max_.Value : 0.0; }
    }

    /// <inheritdoc/>
    public double Min {
      get { return (Count > 0) ? max_.Value : 0.0; }
    }

    /// <inheritdoc/>
    public double Mean {
      get {
        long count = Count;
        return (count > 0) ? sum_.Value/(double) count : 0.0;
      }
    }

    /// <inheritdoc/>
    public double StandardDeviation {
      get { return (Count > 0) ? Math.Sqrt(Variance) : 0.0; }
    }

    /// <summary>
    /// Clears all the recorded values
    /// </summary>
    public void Clear() {
      sample_.Clear();
      count_.Exchange(0);
      max_.Exchange(0);
      min_.Exchange(0);
      sum_.Exchange(0);
      variance_.Exchange(new double[] {-1, 0});
    }

    /// <summary>
    /// Adds a recorded value.
    /// </summary>
    /// <param name="value">The length of the value.</param>
    public void Update(int value) {
      Update((long) value);
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

    double Variance {
      get {
        long count = Count;
        return (count <= 1) ? 0.0 : variance_.Value[1]/(count - 1);
      }
    }

    /// <summary>
    /// Set the maximum recorded value value.
    /// </summary>
    /// <param name="value">
    /// The value that is potentially the maximum value.
    /// </param>
    void SetMax(long value) {
      bool done = false;
      while (!done) {
        long current_max = (long) max_;

        // check if we already have the maximum value.
        done = current_max >= value;
        if (!done) {
          // atomically set the maximum value if it is still the value that we
          // use in the check above.
          max_.CompareExchange(current_max, value);
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
        long current_min = (long) min_;

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
        double[] new_values = new double[2];
        if (old_values[0] == -1) {
          new_values[0] = value;
          new_values[1] = 0;
        } else {
          double old_m = old_values[0];
          double old_s = old_values[1];

          double new_m = old_m + ((value - old_m)/Count);
          double new_s = old_s + ((value - old_m)*(value - new_m));

          new_values[0] = new_m;
          new_values[1] = new_s;
        }
        done = variance_.CompareSet(old_values, new_values);
      }
    }

    /// <summary>
    /// Get the number of values recorded.
    /// </summary>
    /// <returns>The number of values recorded.</returns>
    public long Count {
      get { return (long) count_; }
    }
  }
}
