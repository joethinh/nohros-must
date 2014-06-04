using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A central registry and factory for metric instances.
  /// </summary>
  public class AsyncMetricsRegistry : AbstractMetricsRegistry
  {
    readonly Clock clock_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/>
    /// that uses the default clock to mark the passage of time.
    /// </summary>
    public AsyncMetricsRegistry()
      : this(new UserTimeClock()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/>
    /// that uses the given clock to mark the passage of time.
    /// </summary>
    /// <param name="clock">
    /// The <see cref="Clock"/> used to mark the passage of time.
    /// </param>
    public AsyncMetricsRegistry(Clock clock) {
      clock_ = clock;
    }
    #endregion
  }
}
