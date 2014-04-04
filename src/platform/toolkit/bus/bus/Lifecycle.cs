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
    /// A new instance will be returned fro each call.
    /// </summary>
    InstancePerCall
  }
}
