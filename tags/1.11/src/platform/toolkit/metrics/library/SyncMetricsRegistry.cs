using System;

namespace Nohros.Metrics
{
  /// <summary>
  /// A central registry and factory for metric instances.
  /// </summary>
  public class SyncMetricsRegistry : AbstractMetricsRegistry
  {
    readonly Clock clock_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/>
    /// that uses the default clock to mark the passage of time.
    /// </summary>
    public SyncMetricsRegistry() : this(new UserTimeClock()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncMetricsRegistry"/>
    /// that uses the given clock to mark the passage of time.
    /// </summary>
    /// <param name="clock">
    /// The <see cref="Clock"/> used to mark the passage of time.
    /// </param>
    public SyncMetricsRegistry(Clock clock) {
      clock_ = clock;
    }
    #endregion
  }
}
