using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Nohros.Collections;
using Nohros.Concurrent;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An exponentially-decaying random sample of longs. Uses Cormode et al's
  /// forward-decaying priority resevoir sampling method to produce a
  /// statistically representative sample, exponentially biased towards newer
  /// entries.
  /// </summary>
  /// <remarks>
  /// Cormode et al. Forward Decay: A Practical Time Decay Model for
  /// Streaming Systems. ICDE '09: Proceedings of the 2009 IEEE International
  /// Conference on Data Engineering (2009).
  /// http://www.research.att.com/people/Cormode_Graham/library/publications/CormodeShkapenyukSrivastavaXu09.pdf
  /// </remarks>
  public class ExponentiallyDecayingSample : ISample
  {
    struct Sample
    {
      long value;
      long timestamp;
    }

    const long kRescaleThreshold = 3600000000000;
    readonly double alpha_;
    readonly Clock clock_;
    readonly int resevoir_size_;
    readonly long start_time_;
    readonly Dictionary<double, long> values_;
    AtomicLong count_;
    AtomicLong next_scale_time_;
    Mailbox<Sample> mailbox_; 

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ExponentiallyDecayingSample"/> class by using the specified
    /// resevoir size and exponential decay factor.
    /// </summary>
    /// <param name="resevoir_size">The number of samples to keep in the
    /// sampling resevoir.</param>
    /// <param name="alpha">The exponential decay factor; the higher this is,
    /// the more biased the sample will be towards newer values.</param>
    public ExponentiallyDecayingSample(int resevoir_size, double alpha, Clock clock) {
      count_ = new AtomicLong(0);
      next_scale_time_ = new AtomicLong(0);
      alpha_ = alpha;
      resevoir_size_ = resevoir_size;
      clock_ = clock;
      values_ = new Dictionary<double, long>();
      start_time_ = CurrentTimeInSeconds();
      next_scale_time_.Value = clock.Tick + kRescaleThreshold;
    }
    #endregion

    /// <inheritdoc/>
    public void Update(long value) {
      Update(value, CurrentTimeInSeconds);
    }

    public long CurrentTimeInSeconds {
      get { TimeUnitHelper.ToSeconds(clock_.Time, TimeUnit.Miliseconds); }
    }

    public Snapshot Snapshot {
      get { return new Snapshot(values_.Values); }
    }

    /// <inheritdoc/>
    public int Size {
      get { return (int) Math.Min(resevoir_size_, (long) count_); }
    }

    /// <summary>
    /// Adds an old value with fixed timestamp to the sample.
    /// </summary>
    /// <param name="value">The value to be added.</param>
    /// <param name="timestamp">The epoch timestamp of <paramref name="value"/>
    /// in seconds.</param>
    public void Update(long value, long timestamp) {
      double priority = Weight(timestamp - start_time_)/rand_.NextDouble();
      long new_count = ++count_;
      if (new_count <= resevoir_size_) {
        values_.Add(priority, value);
      } else {
        double first = values_.;
        // TODO: double first = value.FirstKey();
        if (first < priority) {
          // TODO:
          //if (values.putIfAbsent(priority, value) == null) {
          //while (values.remove(first) == null) {
          //first = values.firstKey();
          //}
          //}
        }
      }

      long now = Clock.NanoTime;
      long next = (long) next_scale_time_;
      if (now >= next) {
        Rescale(now, next);
      }
    }

    void Rescale(long now, long next) {
    }

    double Weight(long t) {
      return Math.Exp(alpha_*t);
    }
  }
}
