using System;

namespace Nohros.Bus
{
  /// <summary>
  /// Represent the various lifecycles available for components configured in
  /// the container.
  /// </summary>
  public enum Lifecycle
  {
    /// <summary>
    /// The same instance will be returned each time.
    /// </summary>
    SingleInstance,

    /// <summary>
    /// A new instance will be returned for each call.
    /// </summary>
    InstancePerCall,

    /// <summary>
    /// The instance will be singleton for the duration of the unit of work.
    /// In practice this means the processing of a single command and the
    /// events generated during its processing.
    /// </summary>
    /// <remarks>
    /// This option is used only by application that performs transactional
    /// processing. If the container does not support this type of lifecycle
    /// a <see cref="NotSupportedException"/> should be throw when a attempt
    /// to register a component using this lifecycle if performed.
    /// </remarks>
    InstancePerTransaction,
  }
}
