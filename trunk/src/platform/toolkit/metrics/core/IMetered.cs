using System;

namespace Nohros.Toolkit.Metrics
{
  /// <summary>
  /// An object which maintains mean and exponentially-weighted rate.
  /// </summary>
  public interface IMetered
  {
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
    long Count { get; }

    /// <summary>
    /// Gets the fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// fifteen-minute load average in the <c>top</c> Unix command.</remarks>
    double FifteenMinuteRate { get; }

    /// <summary>
    /// Gets the five-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The five-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// five-minute load average in the <c>top</c> Unix command.</remarks>
    double FiveMinuteRate { get; }

    /// <summary>
    /// Gets the fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.
    /// </summary>
    /// <value>The fifteen-minute exponentially-weighted moving average rate at
    /// which events have occurred since the meter was created.</value>
    /// <remarks>This rate has the same exponential decay factor as the
    /// fifteen-minute load average in the <c>top</c> Unix command.</remarks>
    double OneMinuteRate { get; }

    /// <summary>
    /// Gets the mean rate at which events have occurred since the meter was
    /// created.
    /// </summary>
    /// <value>The mean rate at which events have occurred since the meter was
    /// created.</value>
    double MeanRate { get; }
  }
}
