using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An object which maintains mean and exponentially-weighted rate.
  /// </summary>
  public interface IAsyncMetered
  {
    /// <summary>
    /// Gets the fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// fifteen-minute load average in the <c>top</c> Unix command.</remarks>
    void GetFifteenMinuteRate(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the five-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The five-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// five-minute load average in the <c>top</c> Unix command.</remarks>
    void GetFiveMinuteRate(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// fifteen-minute load average in the <c>top</c> Unix command.</remarks>
    void GetOneMinuteRate(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the mean rate at which events have occurred since the meter was
    /// created.
    /// </summary>
    /// <value>The mean rate at which events have occurred since the meter was
    /// created.</value>
    void GetMeanRate(DoubleMetricCallback callback);

    /// <summary>
    /// Gets the meter's rate unit.
    /// </summary>
    /// <returns></returns>
    TimeUnit RateUnit { get; }

    /// <summary>
    /// Gets the event type of events the meter is measuring.
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// Gets the number of events which have been marked.
    /// </summary>
    void GetCount(LongMetricCallback callback);
  }
}
