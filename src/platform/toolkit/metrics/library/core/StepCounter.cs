﻿
namespace Nohros.Metrics
{
  /// <summary>
  /// A <see cref="ICounter"/> implementation that is mapped to a particular
  /// step interval. The value returned is the rate for the previous interval
  /// as defined by the step.
  /// </summary>
  public class StepCounter : AbstractMetric, ICounter, IStepMetric
  {
    long prev_count_;
    long curr_count_;
    long prev_tick_;
    long curr_tick_;

    /// <summary>
    /// Initializes a new instance of the <see cref="StepCounter"/> class
    /// by using the given <see cref="MetricConfig"/>.
    /// </summary>
    /// <param name="config">
    /// A <see cref="MetricConfig"/> object containing the configuration
    /// that should be used by the <see cref="StepCounter"/> object.
    /// </param>
    public StepCounter(MetricConfig config) : this(config, 0) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepCounter"/> class
    /// by using the given <see cref="MetricConfig"/>,
    /// <see cref="MetricContext"/> and <paramref name="initial"/> value.
    /// </summary>
    /// <param name="config">
    /// A <see cref="MetricConfig"/> object containing the configuration
    /// that should be used by the <see cref="StepCounter"/> object.
    /// </param>
    /// <param name="initial">
    /// The initial value of the counter.
    /// </param>
    public StepCounter(MetricConfig config, long initial)
      : this(config, initial, MetricContext.ForCurrentProcess) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepCounter"/> class
    /// by using the given <see cref="MetricConfig"/> and
    /// <see cref="MetricContext"/>.
    /// </summary>
    /// <param name="config">
    /// A <see cref="MetricConfig"/> object containing the configuration
    /// that should be used by the <see cref="StepCounter"/> object.
    /// </param>
    /// <param name="context">
    /// The <see cref="MetricContext"/> to be used by the counter.
    /// </param>
    public StepCounter(MetricConfig config, MetricContext context)
      : this(config, 0, context) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepCounter"/> class
    /// by using the given <see cref="MetricConfig"/>, initial value and
    /// <see cref="MetricContext"/>.
    /// </summary>
    /// <param name="config">
    /// A <see cref="MetricConfig"/> object containing the configuration
    /// that should be used by the <see cref="StepCounter"/> object.
    /// </param>
    /// <param name="initial">
    /// The initial value of the counter.
    /// </param>
    /// <param name="context">
    /// The <see cref="MetricContext"/> to be used by the counter.
    /// </param>
    public StepCounter(MetricConfig config, long initial, MetricContext context)
      : base(config.WithAdditionalTag(MetricType.Normalized.AsTag()), context) {
      prev_count_ = initial;
      curr_count_ = initial;
      prev_tick_ = curr_tick_ = context.Tick;
    }

    /// <summary>
    /// Creates a new counter by using the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static StepCounter Create(string name) {
      return new StepCounter(new MetricConfig(name));
    }

    /// <summary>
    /// Creates a new counter by using the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="initial"></param>
    /// <returns></returns>
    public static StepCounter Create(string name, int initial) {
      return new StepCounter(new MetricConfig(name), initial);
    }

    /// <inheritdoc/>
    public void Decrement() {
      Decrement(-1);
    }

    /// <inheritdoc/>
    public void Decrement(long n) {
      context_.Send(() => Update(-n));
    }

    /// <inheritdoc/>
    public void Increment() {
      Increment(1);
    }

    /// <inheritdoc/>
    public void Increment(long n) {
      context_.Send(() => Update(n));
    }

    public void OnStep() {
      context_.Send(() => {
        prev_count_ = curr_count_;
        prev_tick_ = curr_tick_;
      });
    }

    /// <inheritdoc/>
    protected internal override Measure Compute(long tick) {
      curr_tick_ = tick;
      double delta = curr_tick_ - prev_tick_;
      return CreateMeasure((curr_count_ - prev_count_)/delta);
    }

    void Update(long delta) {
      curr_count_ += delta;
      if (curr_count_ < 0) {
        curr_count_ = 0;
      }
    }
  }
}