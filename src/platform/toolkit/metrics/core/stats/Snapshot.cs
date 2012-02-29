using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// A statistical snapshot of a <see cref="Snapshot"/>.
  /// </summary>
  public class Snapshot
  {
    const double MEDIAN_Q = 0.5;
    const double P75_Q = 0.75;
    const double P95_Q = 0.95;
    const double P98_Q = 0.98;
    const double P99_Q = 0.99;
    const double P999_Q = 0.999;

    readonly double[] values_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Snapshot"/> class by using
    /// the specifid collection of long values.
    /// </summary>
    /// <param name="values">An unordered set of values in the sample.</param>
    public Snapshot(ICollection<long> values) {
      values_ = new double[values.Count];
      int i=0;
      foreach (double l in values) {
        values_[i++] = (double)l;
      }
      Array.Sort<double>(values_);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Snapshot"/> class by using
    /// the specified array of values.
    /// </summary>
    /// <param name="values">An unordered set of values in the sample.</param>
    public Snapshot(double[] values) {
      values_ = new double[values.Length];
      Array.Copy(values, values_, values.Length);
      Array.Sort<double>(values_);
    }
    #endregion

    /// <summary>
    /// Gets the value at the given quantile.
    /// </summary>
    /// <param name="quantile">A quantile, in <c>[0...1]</c></param>
    /// <returns>The value in the distribution at <paramref name="quantile"/>.
    /// </returns>
    public double Quantile(double quantile) {
      return this[quantile];
    }

    /// <summary>
    /// Gets the value at the given quantile.
    /// </summary>
    /// <param name="quantile">A quantile, in <c>[0...1]</c></param>
    /// <returns>The value in the distribution at <paramref name="quantile"/>.
    /// </returns>
    public double this[double quantile] {
      get{
        if (quantile < 0.0 || quantile > 1.0) {
          throw new ArgumentOutOfRangeException("quantile");
        }

        int length = values_.Length;
        if(length == 0) {
          return 0.0;
        }

        double pos = quantile * (length + 1);
        if (pos < 1) {
          return values_[0];
        }

        if (pos >= values_.Length) {
          return values_[values_.Length - 1];
        }

        double lower = values_[(int)pos - 1];
        double upper = values_[(int)pos];
        return lower + (pos - Math.Floor(pos)) * (upper - lower);
      }
    }

    /// <summary>
    /// Gets the number of items in the snapshot.
    /// </summary>
    public int Size {
      get {
        return values_.Length;
      }
    }

    /// <summary>
    /// Gets hte median value in the distribution.
    /// </summary>
    public double Median {
      get {
        return this[MEDIAN_Q];
      }
    }

    /// <summary>
    /// Gets the value at 75th percentile in the distribution.
    /// </summary>
    /// <value>The 75th percentile in the distribution.</value>
    public double Percentile75 {
      get {
        return this[P75_Q];
      }
    }

    /// <summary>
    /// Gets the value at 95th percentile in the distribution.
    /// </summary>
    /// <value>The 95th percentile in the distribution.</value>
    public double Percentile95 {
      get {
        return this[P95_Q];
      }
    }

    /// <summary>
    /// Gets the value at 98th percentile in the distribution.
    /// </summary>
    /// <value>The 98th percentile in the distribution.</value>
    public double Percentile98 {
      get {
        return this[P98_Q];
      }
    }

    /// <summary>
    /// Gets the value at 99th percentile in the distribution.
    /// </summary>
    /// <value>The 99th percentile in the distribution.</value>
    public double Percentile99 {
      get {
        return this[P99_Q];
      }
    }

    /// <summary>
    /// Gets the value at 99.9h percentile in the distribution.
    /// </summary>
    /// <value>The 99.9th percentile in the distribution.</value>
    public double Percentile999 {
      get {
        return this[P999_Q];
      }
    }

    /// <summary>
    /// Get the entire set of values in the snapshot.
    /// </summary>
    /// <value>The entire set of values in snapshot.</value>
    public double[] Values {
      get {
        int length = values_.Length;
        double[] copy = new double[length];
        values_.CopyTo(copy, 0);

        return copy;
      }
    }
  }
}
